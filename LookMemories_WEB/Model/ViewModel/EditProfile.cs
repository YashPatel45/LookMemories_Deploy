using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Edit proile
    public class EditProfile
    {
        //EDIT profile
        public EditProfile()
        {
            UserProfileVM = new EidtUserProfileViewModel();
            SQVM = new SecurityQuestionViewModel();
            RSVM = new ResetPasswordViewModel();
        }

        public EidtUserProfileViewModel UserProfileVM { get; set; }

        public SecurityQuestionViewModel SQVM { get; set; }

        public ResetPasswordViewModel RSVM { get; set; }
    }
}
