using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using nobot.Services;
using System.Collections.Generic;
using Microsoft.Bot.Builder.FormFlow;
using nobot.Models;
using System.Threading;

namespace nobot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public List<string> DialogOptions = new List<string>() { "Create a Policy", "Ask a Question", "No" };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            //await Conversation.SendAsync(activity, MakeRootDialog);
            
           return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            if(activity.Text == DialogOptions[0])
            {
                await CreatePolicy(context);
            }
            else if(activity.Text == DialogOptions[1])
            {
                await QnA(context);
            }
            else if(activity.Text == DialogOptions[2])
            {

            }
           // context.Wait(MessageReceivedAsync);
        }

        //[LuisIntent("")]
        public async Task QnA(IDialogContext context)
        {
            PromptDialog.Text(context, AnswerQuestionAsync, "What is your question?");
        }

        public async Task AnswerQuestionAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var question = await argument;
            var answer = await ConversationService.GetQuestionAnswer(question);
            await context.PostAsync(answer);

            PromptDialog.Confirm(context, ConfirmIfAnswered, "Is your question answered?");
        }

        public async Task ConfirmIfAnswered(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;

            if (confirmation)
            {
                await context.PostAsync("Great!");
                IMessageActivity msg = context.MakeMessage();
                msg.Attachments.Add(ConversationService.GetWelcomeCard());
                await context.PostAsync(msg);
            }
            else
            {
                await context.PostAsync("Sorry. Please contact customer care for more info.");
            }
        }

        public async Task CreatePolicy(IDialogContext context)
        {
            context.Call(FormDialog.FromForm(Policy.BuildPolicyForm, FormOptions.PromptInStart), CreatePolicyResume);
        }

        public async Task CreatePolicyResume(IDialogContext context, IAwaitable<Policy> result)
        {
            Policy policy = await result;
            context.PrivateConversationData.SetValue(Policy.NAME, policy);
            PromptDialog.Confirm(context, PolicySubmission, $"Do you confirm to create the Policy?");
            
        }
        
        private static async Task PolicySubmission(IDialogContext context, IAwaitable<bool> result)
        {
            bool confirmation = await result;
            if (confirmation)
            {
                Policy policy;
                if (context.PrivateConversationData.TryGetValue(Policy.NAME, out policy))
                {
                    await context.PostAsync("Thanks for the details. Your policy has been submitted.");
                }
                //await context.Forward(new ClaimsDialog(), ResumeAfter, context, CancellationToken.None);
                //context.Done($"Thanks for the details. Your policy has been created. \n");
            }
            IMessageActivity msg = context.MakeMessage();
            msg.Attachments.Add(ConversationService.GetWelcomeCard());
            await context.PostAsync(msg);
        }
    }
}