using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nobot.Models
{

    [Serializable]
    public class Policy
    {
        public static readonly string NAME = "Policy";
        public enum GenderOptions
        {
            Male ,
            Female
        };
        public enum ChoiceOptions { Yes, No };

        [Prompt("What is your name?")]
        [Describe("Customer name")]
        public string CustomerName;

        [Prompt("What is your age?")]
        [Describe("Customer age")]
        public int CustomerAge;

        [Prompt("What is your gender?{||}")]
        [Describe("Customer Gender")]
        public GenderOptions? Gender;

        [Prompt("Do you smoke?{||}")]
        [Describe("Customer smokes")]
        public ChoiceOptions? CustomerSmokes;

        internal static IForm<Policy> BuildPolicyForm()
        {
            return new FormBuilder<Policy>()
                    .AddRemainingFields()
                    .Confirm("No verification will be shown", state => false)
                    //.OnCompletion(processDate)
                    .Build();
        }
    }

    
}