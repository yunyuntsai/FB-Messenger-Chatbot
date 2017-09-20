using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using System.Web;

using Microsoft.Bot.Builder.Dialogs;

using Microsoft.Bot.Builder.FormFlow;

using Microsoft.Bot.Builder.Luis;

using Microsoft.Bot.Builder.Luis.Models;

using Microsoft.Bot.Connector;



namespace My_Bot_Application.Dialogs

{
    [LuisModel("d859cfd4-dd77-4565-934d-b72364db1338", "2b5c5d73d03a4b108a1b66306acff9d6")]

    [Serializable]


    public class RootDialog : LuisDialog<object>
    {

        private const string EntityTime = "時間";
        private const string EntityDate = "日期";
        private const string EntityPlace = "地點";
        private const string EntityAgenda = "流程";
        private const string EntityWho = "人稱::誰";
        private const string EntityI = "人稱::我";
        private const string EntityYou = "人稱::你";
        private const string EntityPretty = "外表::女";
        private const string EntityHandsome = "外表::男";
        private const string EntityName = "名字";

        private IList<string> title = new List<string> { "“9/2 DAY1 知性課程”", "“9/3 DAY2 微王國之旅”" };
        private IList<string> text = new List<string> { "“9:00~9:30 相見歡\n\n9:30~10:30 MS Transformation Journey\n\n10:30~12:30 主題1,2\n\n12:30~14:00 Lunch & IT Booth\n\n14:30~15:00 主題3(Nasa)\n\n15:30~16:00 主題4(skype for business)\n\n16:00~18:00 啟程\n\n18:30~19:30 微王國的盛宴\n\n19:30~19:45 公主加冕儀式\n\n19:45~21:45 微慶典聖火點燃”", "“7:30~9:00 破曉之時\n\n9:00~12:00 騎士出任務\n\n12:00~14:00 微王國饗宴\n\n14:00~15:30 水慶典\n\n15:30~16:00 騎士著裝\n\n1610-1640 探索自我\n\n1640~1700 回程”" };
        private int count = 0;


        [LuisIntent("None")]

        public async Task None(IDialogContext context, LuisResult result)
        {
            
            string message = $"Sorry, 我不知道你在說什麼耶. 好想睡覺喔~";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("確認")]

        public async Task Check(IDialogContext context, LuisResult result)
        {
            //string message = $"太好了都記起來了吧! 期待在領袖營看到你~";
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://i.telegraph.co.uk/multimedia/archive/01204/pig1_1204022c.jpg"));
            cardImages.Add(new CardImage(url: "http://mepopedia.com/~web103-a/final/final-1025445058/img/ca3a41568bb573243b80e5cd584b3fb9.jpg"));
            IList<string> text = new List<string> { "9:00~9:30 :\n\n[相見歡]\n\n9:30~10:30 :\n\n[MS Journey]\n\n10:30~12:30 :\n\n[主題1 & 2]\n\n12:30~14:00 :\n\n[Lunch & IT Booth]\n\n14:30~15:00 :\n\n[主題3--Nasa]\n\n15:30~16:00 :\n\n[主題4--skype]\n\n16:00~18:00 : [啟程]\n\n18:30~19:30 :[王國盛宴]\n\n19:30~19:45 :\n\n[公主加冕儀式]\n\n19:45~21:45 :\n\n[微慶典聖火點燃]”", "“7:30~9:00 : [破曉之時]\n\n9:00~12:00 : [騎士出任務]\n\n12:00~14:00 : [王國饗宴]\n\n14:00~15:30 : [水慶典]\n\n15:30~16:00 : [騎士著裝]\n\n1610-1640 : [探索自我]\n\n1640~1700 : [回程]”" };
            if (result.Query == "Check0")
            {
                await context.PostAsync(text[0]);
                var resultMessage = context.MakeMessage();

                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                resultMessage.Attachments = new List<Attachment>();



                for (int i = 0; i < 2; i++)

                {

                    //var random = new Random(i);
                    HeroCard heroCard = new HeroCard()

                    {

                        Title = title[i],

                        Images = cardImages.GetRange(i, 1),


                        Buttons = new List<CardAction>()

                        {

                            new CardAction()

                            {

                                Title = "Check",

                                Type = ActionTypes.ImBack,

                                Value = "Check"+ Convert.ToString(i),


                            }

                        }

                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());

                }

                await context.PostAsync(resultMessage);
            }
            else
            {
                await context.PostAsync(text[1]);
                context.Call(new PlaceQuery(), this.ResumePlaceDialog);
            }
          
           
        }

        [LuisIntent("Help")]

        public async Task Help(IDialogContext context, LuisResult result)
        {
            var resultMessage = context.MakeMessage();

            resultMessage.AttachmentLayout = AttachmentLayoutTypes.List;

            resultMessage.Attachments = new List<Attachment>();

            List<CardAction> cardButtons = new List<CardAction>();

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://www.mariowiki.com/images/9/9c/Peachicon.png"));
           

            CardAction p1Button = new CardAction()
            {
                Title = "詢問時間",
                Value = "領袖營幾號",
                Type = ActionTypes.ImBack

            };
            CardAction p2Button = new CardAction()
            {
                Title = "詢問地點",
                Value = "領袖營在哪",
                Type = ActionTypes.ImBack

            };
            CardAction p3Button = new CardAction()
            {
                Title = "詢問活動流程",
                Value = "領袖營活動",
                Type = ActionTypes.ImBack

            };

            cardButtons.Add(p1Button);
            cardButtons.Add(p2Button);
            cardButtons.Add(p3Button);

            ThumbnailCard plCard = new ThumbnailCard()
            {
                Title = $"點點這邊的選項吧!",
                Buttons = cardButtons,
                 Images = cardImages

            };
            Attachment plAttachment = plCard.ToAttachment();
            resultMessage.Attachments.Add(plAttachment);
            await context.PostAsync(resultMessage);

            context.Wait(this.MessageReceived);
        }
        [LuisIntent("問好")]

        public async Task Askhi(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await context.PostAsync($"Hi 我是微美公主~ 今年三歲 很高興認識你");

            await context.PostAsync($"你可以問我領袖營的問題唷!\n\n你可以說'拜託美麗的公主幫幫我'");
                 
            context.Wait(this.MessageReceived);
        }

       
        [LuisIntent("詢問領袖營時程")]

        public async Task AskTime(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

            //await context.PostAsync($"這你就問對人了啦");

            EntityRecommendation DateEntityRecommendation;
            EntityRecommendation TimeEntityRecommendation;

            if (result.TryFindEntity(EntityDate, out DateEntityRecommendation))
            {
                await context.PostAsync($"讓我來告訴你領袖營'{DateEntityRecommendation.Entity}'吧!");
                await context.PostAsync($"答案是9/2 & 9/3 ~");
                context.Call(new Query(), this.AgeDialogResumeAfter);
            }
            else if (result.TryFindEntity(EntityTime, out TimeEntityRecommendation))
            {
                await context.PostAsync($"是九點開始喔");
                context.Call(new PlaceQuery(), this.ResumePlaceDialog);
            }

        }

        [LuisIntent("詢問領袖營地點")]

        public async Task AskPlace(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {


            EntityRecommendation PlaceEntityRecommendation;

            await context.PostAsync($"這你就問對人了啦~");
            if (result.TryFindEntity(EntityPlace, out PlaceEntityRecommendation))
            {

                await context.PostAsync($"第一天在台灣微軟總部19樓\n\n第二天在新竹的溫馨農場\n\n");
                
                context.Call(new PlaceQuery(), this.ResumePlaceDialog);


            }


        }

        [LuisIntent("詢問領袖營活動流程")]

        public async Task AskAgenda(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation AgendaEntityRecommendation;
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://i.telegraph.co.uk/multimedia/archive/01204/pig1_1204022c.jpg"));
            cardImages.Add(new CardImage(url: "http://mepopedia.com/~web103-a/final/final-1025445058/img/ca3a41568bb573243b80e5cd584b3fb9.jpg"));

            if (result.TryFindEntity(EntityAgenda, out AgendaEntityRecommendation))

            {

                await context.PostAsync($"正在查看'{AgendaEntityRecommendation.Entity}'...");

                var resultMessage = context.MakeMessage();

                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                resultMessage.Attachments = new List<Attachment>();



                for (int i = 0; i < 2; i++)

                {

                    //var random = new Random(i);
                    HeroCard heroCard = new HeroCard()

                    {

                        Title = title[i],

                        Images = cardImages.GetRange(i, 1),

                
                        Buttons = new List<CardAction>()

                        {

                            new CardAction()

                            {

                                Title = "Check",

                                Type = ActionTypes.ImBack,

                                Value = "Check"+ Convert.ToString(i),


                            }

                        }

                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());

                }

                await context.PostAsync(resultMessage);
            
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("讚美")]

        public async Task Chat(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

            EntityRecommendation Who;
            EntityRecommendation I;
            EntityRecommendation You;
            EntityRecommendation Name;
            EntityRecommendation pretty;
            EntityRecommendation handsome;

            if (result.TryFindEntity(EntityWho, out Who))
            {
                if (result.TryFindEntity(EntityPretty, out pretty)) await context.PostAsync($"雖然我很漂亮,不過最漂亮的的當然是我們的GraceMa囉!");
                else
                {
                    var resultMessage = context.MakeMessage();
                    await context.PostAsync($"哎呀真害羞~");
                    resultMessage.Attachments.Add(new Attachment()
                    {
                        ContentUrl = "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png",
                        ContentType = "image/png",
                        Name = "Bender_Rodriguez.png"
                    });
                    await context.PostAsync(resultMessage);
                }
            }
            else if (result.TryFindEntity(EntityI, out I))
            {
                if (result.TryFindEntity(EntityPretty, out pretty)) await context.PostAsync($"恩~~還不錯啦,差我一點點><");
                else if (result.TryFindEntity(EntityHandsome, out handsome)) await context.PostAsync($"哈哈真想認識你~");
            }
            else if (result.TryFindEntity(EntityYou, out You))
            {
                if (result.TryFindEntity(EntityPretty, out pretty)) await context.PostAsync($"這就不用多說啦,我可是網beauty呢!");
            }
            else if (result.TryFindEntity(EntityName, out Name))
            {
                if (Name.Entity == "昀芸" && result.TryFindEntity(EntityPretty, out pretty)) await context.PostAsync($"創造我的人當然超{pretty.Entity}");
                else if (Name.Entity == "York" && result.TryFindEntity(EntityHandsome, out handsome)) await context.PostAsync($"創造我的人當然超{handsome.Entity}");
                else if (result.TryFindEntity(EntityPretty, out pretty)) await context.PostAsync($"我也覺得{Name.Entity}很{pretty.Entity}");
                else if (result.TryFindEntity(EntityHandsome, out handsome)) await context.PostAsync($"我也覺得{Name.Entity}很{handsome.Entity}");

            }
            context.Wait(this.MessageReceived);
        }


        [LuisIntent("聊天")]

        public async Task Chat1(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

            if (result.Query == "講個笑話")
            {
                await context.PostAsync($"你知道是誰殺了牛奶嘛? 答案是綠豆~因為'綠豆沙牛奶'XD");
               
            }
       
         
       
        }

      
        private async Task ResumePlaceDialog(IDialogContext context, IAwaitable<string> result)
        {
            try
            {

                string s = await result;
                if(s == "yes" || s== "Yes" || s=="y" || s=="想")
                {
                    var resultMessage = context.MakeMessage();

                    resultMessage.AttachmentLayout = AttachmentLayoutTypes.List;

                    resultMessage.Attachments = new List<Attachment>();

                    List<CardAction> cardButtons = new List<CardAction>();

                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://www.mariowiki.com/images/9/9c/Peachicon.png"));


                    CardAction p1Button = new CardAction()
                    {
                        Title = "詢問時間",
                        Value = "領袖營幾號",
                        Type = ActionTypes.ImBack

                    };
                    CardAction p2Button = new CardAction()
                    {
                        Title = "詢問地點",
                        Value = "領袖營在哪",
                        Type = ActionTypes.ImBack

                    };
                    CardAction p3Button = new CardAction()
                    {
                        Title = "詢問活動流程",
                        Value = "領袖營活動",
                        Type = ActionTypes.ImBack

                    };

                    cardButtons.Add(p1Button);
                    cardButtons.Add(p2Button);
                    cardButtons.Add(p3Button);

                    ThumbnailCard plCard = new ThumbnailCard()
                    {
                        Title = $"點點這邊的選項吧!",
                        Buttons = cardButtons,
                        Images = cardImages

                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    resultMessage.Attachments.Add(plAttachment);
                    await context.PostAsync(resultMessage);

                    context.Wait(this.MessageReceived);
                }
                else 
                {
                    await context.PostAsync($"不想的話可以跟我聊聊天喔~");
                    var resultMessage = context.MakeMessage();

                    resultMessage.AttachmentLayout = AttachmentLayoutTypes.List;

                    resultMessage.Attachments = new List<Attachment>();

                    List<CardAction> cardButtons = new List<CardAction>();

                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://www.mariowiki.com/images/9/9c/Peachicon.png"));


                    CardAction p1Button = new CardAction()
                    {
                        Title = "講個笑話",
                        Value = "講個笑話",
                        Type = ActionTypes.ImBack

                    };
                    CardAction p2Button = new CardAction()
                    {
                        Title = "誰最漂亮",
                        Value = "誰最漂亮",
                        Type = ActionTypes.ImBack

                    };
                    CardAction p3Button = new CardAction()
                    {
                        Title = "誰最帥",
                        Value = "誰最帥",
                        Type = ActionTypes.ImBack

                    };
                    


                    cardButtons.Add(p1Button);
                    cardButtons.Add(p2Button);
                    cardButtons.Add(p3Button);

                    ThumbnailCard plCard = new ThumbnailCard()
                    {
                        Title = $"點我點我!",
                        Buttons = cardButtons,
                        Images = cardImages

                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    resultMessage.Attachments.Add(plAttachment);
                    await context.PostAsync(resultMessage);

                    context.Wait(this.MessageReceived);
                }
                
                
            }

            catch (TooManyAttemptsException)

            {

                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                //await this.Askhi(context);

            }

        }

        private async Task AgeDialogResumeAfter(IDialogContext context, IAwaitable<string> result)

        {

            try

            {
                string s =  await result;
                if (s == "yes" || s == "Yes" || s == "y" || s == "想")
                {
                
                    await context.PostAsync("答案是九點開始喔~");
                    context.Call(new PlaceQuery(), this.ResumePlaceDialog);

                }
                else
                {
                    context.Call(new PlaceQuery(), this.ResumePlaceDialog);

                }

            }

            catch (TooManyAttemptsException)

            {

                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }

            finally

            {
               
            }

        }
        /*public class RootDialog : IDialog<object>
        {
            public Task StartAsync(IDialogContext context)
            {
                context.Wait(MessageReceivedAsync);

                return Task.CompletedTask;
            }

            private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
            {
                var activity = await result as Activity;

                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                await context.PostAsync($"You sent {activity.Text} which was {length} characters");

                context.Wait(MessageReceivedAsync);
            }
        }*/
    }
}