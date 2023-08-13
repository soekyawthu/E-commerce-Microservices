namespace EventBus.Messages;

public class MessageBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateTime CreationDate { get; private set; } = DateTime.Now;
}