using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_Bot_Application
{
    using System;

    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]

    public class ResumePlaceQuery
    {

        [Prompt("Please enter Date {&}")]

        [Optional]

        public string Date { get; set; }

    }

}