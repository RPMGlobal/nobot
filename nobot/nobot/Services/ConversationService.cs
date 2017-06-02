using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace nobot.Services
{
    public class ConversationService
    {

        public static Attachment GetWelcomeCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Health Policy Bot",
                Subtitle = "",
                Text = "Hi, how may I help you?",
                Images = new List<CardImage> { new CardImage("https://media.licdn.com/mpr/mpr/shrinknp_800_800/AAEAAQAAAAAAAAjEAAAAJDg0N2NmNjU1LTQ2OTctNDg3Ni1iMjNhLWExZmE0YTg0NDIxOQ.png") },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.PostBack, "Create Health Policy", value: "Create a Policy"),
                    new CardAction(ActionTypes.PostBack, "FAQ", value: "Ask a Question"),
                    new CardAction(ActionTypes.PostBack, "I am good!", value: "No"),
                }
            };

            return heroCard.ToAttachment();
        }

        public static async Task<string> GetQuestionAnswer(string question)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["QnaURL"] + ConfigurationManager.AppSettings["QnaKBID"] + "/generateAnswer");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["QnaSubscriptionID"]);
            HttpResponseMessage response = await client.PostAsJsonAsync(client.BaseAddress, new { question = question });
            string jsonResult = response.Content.ReadAsStringAsync().Result;
            QnAMakerResults KBResult = JsonConvert.DeserializeObject<QnAMakerResults>(jsonResult);
            return KBResult.Answers.First().Answer; 
            //return "a";          
        }

    }
}