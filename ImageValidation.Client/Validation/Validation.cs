using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ImageValidation.Client.Resources;

namespace ImageValidation.Client.Validation
{
    public class UserInfo : NotifyingObject
    {

        // [Display(Name = "Username")]
        [Required(ErrorMessageResourceName = "Username", ErrorMessageResourceType = typeof(ErrorResources))]        
        [StringLength(128, ErrorMessageResourceName = "UsernameLength", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Username
        {
            get { return GetValue(() => Username); }
            set { SetValue(() => Username, value); }
        }

        // [Display(Name = "Mot de passe")]
        [Required(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(ErrorResources))]
        //[StringLength(20, ErrorMessageResourceName = "Passwordchar20", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Password
        {
            get { return GetValue(() => Password); }
            set { SetValue(() => Password, value); }
        }
    }
}
