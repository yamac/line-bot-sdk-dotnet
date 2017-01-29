using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yamac.LineMessagingApi.AspNetCore.Middleware;
using Yamac.LineMessagingApi.Client;

namespace LinebotEcho
{
    public class LinebotEchoHandler : ILineMessagingRequestHandler
    {
        private readonly ILineMessagingApi _api;

        private readonly ILogger _logger;

        public LinebotEchoHandler(IOptions<LineMessagingMiddlewareOptions> options, ILoggerFactory loggerFactory)
        {
            _api = new LineMessagingApi(options.Value.ChannelAccessToken);
            _logger = loggerFactory.CreateLogger<LinebotEchoHandler>();
        }

        /// <summary>
        /// Handle a request of the LINE Messaging API.
        /// </summary>
        /// <param name="request">A request of the LINE Messaging API.</param>
        public async Task HandleRequestAsync(LineMessagingRequest request)
        {
            // Handle all of events.
            await Task.WhenAll(request.Events.Select(HandleEventAsync));
        }

        /// <summary>
        /// Handle event.
        /// </summary>
        /// <param name="theEvent">Event.</param>
        private async Task HandleEventAsync(Yamac.LineMessagingApi.Event.Event theEvent)
        {
            try
            {
                switch (theEvent.Type)
                {
                    case Yamac.LineMessagingApi.Event.EventType.Message:
                        await HandleMessageEventAsync(theEvent as Yamac.LineMessagingApi.Event.MessageEvent);
                        break;
                    default:
                        _logger.LogInformation($@"HandleEventAsync: Unhandled event={theEvent.Type}");
                        break;
                }
            }
            catch (Exception e)
            {
                // Log exception.
                _logger.LogError($@"HandleEventAsync: Exception={e.Message}");
            }
        }

        /// <summary>
        /// Handle message type of event.
        /// </summary>
        /// <param name="messageEvent">Message event.</param>
        private async Task HandleMessageEventAsync(Yamac.LineMessagingApi.Event.MessageEvent messageEvent)
        {
            switch (messageEvent.Message.Type)
            {
                case Yamac.LineMessagingApi.Event.MessageType.Text:
                    await HandleTextMessageAsync(messageEvent.Message as Yamac.LineMessagingApi.Event.TextMessage, messageEvent.ReplyToken);
                    break;
                default:
                    _logger.LogInformation($@"HandleEventAsync: Unhandled event={messageEvent.Type}, {messageEvent.Message.Type}");
                    break;
            }
        }

        /// <summary>
        /// Handle text type of message event.
        /// </summary>
        /// <param name="textMessage">Text message.</param>
        /// <param name="replyToken">Reply token.</param>
        private async Task HandleTextMessageAsync(Yamac.LineMessagingApi.Event.TextMessage textMessage, string replyToken)
        {
            await ReplyTextMessageAsync($@"Text: Text={textMessage.Text}", replyToken);
        }

        /// <summary>
        /// Reply text message by reply token received via webhook.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="replyToken">Reply token.</param>
        private async Task ReplyTextMessageAsync(string text, string replyToken)
        {
            var message = new Yamac.LineMessagingApi.Message.ReplyMessage(
                replyToken,
                new Yamac.LineMessagingApi.Message.TextMessage
                {
                    Text = text,
                });
            await _api.ReplyMessageAsync(message);
        }
    }
}
