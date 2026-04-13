namespace DiscordTrain.Common;

public interface INotificationCentre
{
    void Send<TMessage>(TMessage message) where TMessage : class;
}
