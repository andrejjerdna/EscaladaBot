using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface IVoiceRepository
{
    Guid AddVoice(AddVoiceModel model);
    AddVoiceModel? GetVoiceModel(Guid id);
}