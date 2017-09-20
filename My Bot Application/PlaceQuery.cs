﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_Bot_Application
{
    using System;

    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Dialogs;
    using System.Threading.Tasks;
    using System.Threading;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class PlaceQuery:IDialog<string>
    {

        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)

        {

            await context.PostAsync("還想知道什麼嗎? yes/no");

            context.Wait(this.MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)

        {

            var message = await result;

            /* If the message returned is a valid name, return it to the calling dialog. */

            if ((message.Text != null) && (message.Text.Trim().Length > 0))

            {
                /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling

                    dialog. */


                context.Done(message.Text);

            }

            /* Else, try again by re-prompting the user. */
            else

            {
                --attempts;

                if (attempts > 0)

                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your name (e.g. 'Bill', 'Melinda')?");

                    context.Wait(this.MessageReceivedAsync);

                }

                else

                {
                    /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the 

                        parent/calling dialog. */

                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }

            }

        }

    }
}