namespace DiscordTrain;

public interface ITrainAnimator
{
    double CurrentSpeed { get; }
    double DesiredSpeed { get; set; }

    void EmergencyStop();
    void Animate();
}