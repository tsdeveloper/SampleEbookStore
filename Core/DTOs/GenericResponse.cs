namespace Core.DTOs;

public class GenericResponse<T>
{
    public MessageResponse Error { get; set; }
    public T Data { get; set; }
}
public class MessageResponse
{
    public string Message { get; set; }
    public int Status { get; set; }
}