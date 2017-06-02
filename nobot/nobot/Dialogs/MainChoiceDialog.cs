using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace nobot.Dialogs
{
    public class MainChoiceDialog : IDialog<object>
    {
        private static readonly List<string> DialogOptions = new List<string> { "1", "2", "3" };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

      


        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;


            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: DialogOptions,
                prompt: "how may I help you?",
                retry: "I didn't understand. Please try again.", //luis dialog
                promptStyle: PromptStyle.Auto
            );


        }
        private async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            string activity = await result;


            if (activity.Equals(DialogOptions[0]))
            {
                await context.PostAsync("What's your question?");
            }
            else if (activity.Equals(DialogOptions[1]))
            {
                await context.PostAsync($"you selected {DialogOptions[1]}");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}