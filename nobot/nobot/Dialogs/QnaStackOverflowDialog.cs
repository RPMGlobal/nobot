using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nobot.Dialogs
{
    [Serializable]
    [QnAMaker("set yout subscription key here", "set your kbid here", "I don't understand this right now! Try another query!", 0.50, 3)]
    public class QnaStackOverflowDialog : QnAMakerDialog
    {
    }
}