using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class AppUser : IdentityUser<int>,ITracker,IKey<int>,IEnable
    {
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [MaxLength(10)]
        public string MelliCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(80)]
        public string Family { get; set; }

        public bool Enable { get; set; }

        public string FullName => $"{Name} {Family}";

        public virtual ICollection<Message> SentMessages { get; set; }

        public virtual ICollection<MessageReceiver> ReceivedMessages { get; set; }


        public int BuildingManagerId { get; set; }
        public virtual BuildingManager BuildingManager { get; set; }


        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int GroupManagerId { get; set; }
        public virtual GroupManager GroupManager { get; set; }


        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }


        public int StudentId { get; set; }
        public virtual Student Student { get; set; }



    }

        public class AppRole : IdentityRole<int> { }

        public class UserToken : IdentityUserToken<int> { }

        public class UserRole : IdentityUserRole<int> { }

        public class UserClaim : IdentityUserClaim<int> { }

        public class UserLogin : IdentityUserLogin<int> { }

        public class UserRoleClaim : IdentityRoleClaim<int> { }
}
