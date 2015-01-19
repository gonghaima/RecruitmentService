
using MvcApplication1.Models.DBModels;
namespace MvcApplication1.Models.ViewModels
{
    public interface IRegisterInterface
    {
        bool register(string roleName);
    }

    public interface IUserInterface
    {
        bool login();
    }
}
