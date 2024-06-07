using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URM.Service.Constants
{
	public static class Messages
	{
		public static string RoleAssignedToUser = "Kullanıcıya Rol Atandı";
		public static string UserRoleWasSuccessfullyRemoved = "Rol Başarıyla Kaldırıldı";
		public static string UserNotFound = "Kullanıcı Bulunamadıı";

		public static string RoleNotRegistered = "Kullanıcıya Ait Rol Yok";
		public static string RoleAlreadyAssigned = "Bu Rol Zaten Kullanıcıya Ait ";
		public static string RoleAlreadyExists = "Bu Rol Zaten Var ";
		public static string RoleDeleted = " Rol Silindi ";
		public static string RoleUpdated = " Rol Güncellendi ";
		public static string RoleAdded = " Rol Eklendi ";
		public static string RoleNotFound = "Güncellenecek Rol Bulunamadı.";

		public static string UnauthorizedAccessOnlyAdmins = "Yetkisiz Erişim. Sadece Yöneticiler bu İşlemi Gerçekleştirebilir.";
		public static string OnlyAdminsCanPerformThisAction = "Sadece Yöneticiler Bu İşlemi Gerçekleştirebilir.";
		public static string OnlyAdminsOrEditorsCanPerformThisAction = "Sadece Yöneticiler Veya Editörler Bu İşlemi Gerçekleştirebilir.";
	}
}
