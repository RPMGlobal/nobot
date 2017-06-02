using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System.Linq;

namespace nobot.Dialogs
{
    [Serializable]
    [QnAMaker("78060476e2724d15b1bf904f687c5ab1", "ed222fea-c558-40b7-a471-2b5803f61e0b")]
    public class FAQDialog : QnAMakerDialog
    {
        // Override to also include the knowledgebase question with the answer on confident matches
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            if (results.Answers.Count > 0)
            {
                var response = results.Answers.First().Answer;
                await context.PostAsync(response);
            }
        }

        // Override to log matched Q&A before ending the dialog
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            
            await context.PostAsync("Do you have any other questions?");
            context.Wait(base.MessageReceivedAsync);
        }
    }
}