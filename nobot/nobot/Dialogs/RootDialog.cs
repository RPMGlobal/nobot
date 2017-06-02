using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using nobot.Services;

namespace nobot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
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
            await QnA(context);
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
                //IMessageActivity msg = context.MakeMessage();
                //msg.Attachments.Add(ConversationService.GetMenuCard());
                //await context.PostAsync(msg);
            }
            else
            {
                await context.PostAsync("Sorry. Please contact customer care for more info.");
            }
        }
    }
}