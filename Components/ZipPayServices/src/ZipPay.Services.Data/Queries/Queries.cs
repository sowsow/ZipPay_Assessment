namespace ZipPay.Services.Data.Queries
{
    public static class Queries
    {
        public static string InsertUser => @"
            INSERT INTO zipuser    AS U
                                              ( id,
                                                emailaddress,
                                                name,
                                                monthlysalary,
                                                monthlyexpenses)
            VALUES                            ( :id,
                                                :emailaddress,
                                                :name,
                                                :monthlysalary,
                                                :monthlyexpenses)
            RETURNING U.*;
        ";

        public static string FindUserById => @"
            SELECT  *
            FROM    zipuser
            WHERE   id              = :id;
        ";

        public static string FindUserByEmailAddress => @"
            SELECT  *
            FROM    zipuser
            WHERE   emailaddress    = :emailaddress;
        ";

        public static string FindAllUsers => @"
            SELECT  *
            FROM    zipuser;
        ";

        public static string InsertAccount => @"
            INSERT INTO account     AS A
                                              ( id,
                                                name,
                                                createdbyuserid)
            VALUES                            ( :id,
                                                :name,
                                                :createdbyuserid)
            RETURNING A.*;
        ";

        public static string FindAccountById => @"
            SELECT  *
            FROM    account
            WHERE   id                  = :id;
        ";

        public static string FindAccountsByUserId => @"
            SELECT  *
            FROM    account
            WHERE   createdbyuserid     = :createdbyuserid;
        ";

        public static string FindAllAccounts => @"
            SELECT  *
            FROM    account;
        ";
    }
}
