
using MvcApplication1.Models.DBModels;
namespace MvcApplication1.Models.ViewModels
{
    public interface IRegisterInterface
    {
        bool register();
    }

    public interface IUserInterface
    {
        bool login();
        void logout();
    }
}
