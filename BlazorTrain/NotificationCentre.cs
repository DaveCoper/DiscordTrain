using CommunityToolkit.Mvvm.Messaging;
using DiscordTrain.Common;

namespace BlazorTrain;

public class NotificationCentre(IMessenger messenger) : INotificationCentre
{
    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        messenger.Send(message);
    }
}
