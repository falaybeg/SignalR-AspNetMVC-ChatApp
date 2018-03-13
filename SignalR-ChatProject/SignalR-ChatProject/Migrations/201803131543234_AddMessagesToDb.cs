namespace SignalR_ChatProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        SendTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
