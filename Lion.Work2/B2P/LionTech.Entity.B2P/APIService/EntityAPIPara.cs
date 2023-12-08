namespace LionTech.Entity.B2P.APIService
{
    public class EntityAPIPara : EntityAPIService
    {
        public EntityAPIPara(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SCMAPB2PSettingSupB2PUser : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBChar UserGender;
            public DBNVarChar UserTitle;
            public DBVarChar UserTel1;
            public DBVarChar UserTel2;
            public DBVarChar UserEmail;
            public DBNVarChar Remark;
            public DBChar IsGrantor;
            public DBVarChar GrantorUserID;
            public DBChar IsDisable;
        }

        public class SCMAPB2PSettingB2PSystemRole : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBNVarChar RoleNMzhTW;
            public DBNVarChar RoleNMzhCN;
            public DBNVarChar RoleNMenUS;
            public DBNVarChar RoleNMthTH;
            public DBNVarChar RoleNMjaJP;
        }
    }
}