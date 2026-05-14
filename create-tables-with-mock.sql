-- Create LeadsClients table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LeadsClients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LeadsClients] (
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [FullName] [nvarchar](200) NOT NULL,
        [Phone] [nvarchar](50) NOT NULL,
        [Email] [nvarchar](200) NULL,
        [Source] [nvarchar](100) NOT NULL,
        [Status] [nvarchar](50) NOT NULL,
        [NextFollowUpAt] [datetime2] NULL,
        [Notes] [nvarchar](2000) NULL,
        [CreatedAt] [datetime2] NOT NULL,
        [UpdatedAt] [datetime2] NOT NULL
    );
    PRINT 'LeadsClients table created.';
END
GO

-- Create Interactions table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Interactions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Interactions] (
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [LeadClientId] [int] NOT NULL,
        [Channel] [nvarchar](50) NOT NULL,
        [Summary] [nvarchar](1000) NOT NULL,
        [InteractionAt] [datetime2] NOT NULL,
        [NextFollowUpAt] [datetime2] NULL,
        CONSTRAINT [FK_Interactions_LeadsClients] FOREIGN KEY ([LeadClientId]) REFERENCES [dbo].[LeadsClients]([Id]) ON DELETE CASCADE
    );
    PRINT 'Interactions table created.';
END
GO

-- Create index on Interactions.LeadClientId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Interactions_LeadClientId' AND object_id = OBJECT_ID(N'[dbo].[Interactions]'))
BEGIN
    CREATE INDEX [IX_Interactions_LeadClientId] ON [dbo].[Interactions] ([LeadClientId]);
    PRINT 'Index created.';
END
GO

-- Insert Mock Data into LeadsClients
INSERT INTO [dbo].[LeadsClients] ([FullName], [Phone], [Email], [Source], [Status], [NextFollowUpAt], [Notes], [CreatedAt], [UpdatedAt])
VALUES 
    ('דוד כהן', '050-1234567', 'david@example.com', 'Website', 'Active', DATEADD(DAY, -1, CAST(GETDATE() AS DATE)), 'עיניין לחדר משחק בסלון', GETDATE(), GETDATE()),
    ('שרה לוי', '052-9876543', 'sarah@example.com', 'Referral', 'Active', CAST(GETDATE() AS DATE), 'לשיחה על פרויקט מטבח', GETDATE(), GETDATE()),
    ('אברהם רוזן', '054-5555555', 'avraham@example.com', 'LinkedIn', 'Pending', DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), 'המתין ללהדיון מחדש', GETDATE(), GETDATE()),
    ('ליאור גולן', '053-7777777', 'lior@example.com', 'Phone Call', 'Active', CAST(GETDATE() AS DATE), 'צריך לשיחה על תקציב', GETDATE(), GETDATE()),
    ('מרים בר', '051-3333333', 'miriam@example.com', 'Website', 'Closed', NULL, 'סיים פרויקט בהצלחה', GETDATE(), GETDATE()),
    ('יוסף אלי', '050-2222222', 'yosef@example.com', 'Referral', 'Active', DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 'חכה לאישור מבן הזוג', GETDATE(), GETDATE()),
    ('רחל משה', '052-8888888', 'rachel@example.com', 'Email', 'Active', CAST(GETDATE() AS DATE), 'הצעה חדשה נשלחה', GETDATE(), GETDATE());

PRINT 'Mock data inserted into LeadsClients.';
GO

-- Insert Mock Data into Interactions
INSERT INTO [dbo].[Interactions] ([LeadClientId], [Channel], [Summary], [InteractionAt], [NextFollowUpAt])
VALUES 
    (1, 'Phone', 'שוחחנו על גדלים וצבעים עבור חדר המשחק', DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -1, CAST(GETDATE() AS DATE))),
    (1, 'Email', 'שלחתי הצעת עיצוב ראה״י', DATEADD(DAY, -1, GETDATE()), CAST(GETDATE() AS DATE)),
    (2, 'WhatsApp', 'שיחה בעברית על חלוקת התקציב', GETDATE(), DATEADD(DAY, 1, CAST(GETDATE() AS DATE))),
    (2, 'Meeting', 'טיור באתר', DATEADD(DAY, -5, GETDATE()), CAST(GETDATE() AS DATE)),
    (3, 'Email', 'שלחתי רעיונות תחיליים', DATEADD(DAY, -7, GETDATE()), DATEADD(DAY, 2, CAST(GETDATE() AS DATE))),
    (4, 'Phone', 'דיון על תקציב וזמנים', DATEADD(DAY, -2, GETDATE()), CAST(GETDATE() AS DATE)),
    (4, 'Email', 'שלחתי הצעה מפורטת', GETDATE(), DATEADD(DAY, 1, CAST(GETDATE() AS DATE))),
    (5, 'Meeting', 'סיום פרויקט והעברת מפתחות', DATEADD(DAY, -10, GETDATE()), NULL),
    (6, 'Phone', 'שיחה ראשונית', DATEADD(DAY, -4, GETDATE()), DATEADD(DAY, 1, CAST(GETDATE() AS DATE))),
    (7, 'Email', 'שלחתי קטלוג המוצרים', DATEADD(DAY, -1, GETDATE()), CAST(GETDATE() AS DATE));

PRINT 'Mock data inserted into Interactions.';
GO

-- Verify data
SELECT 'LeadsClients Count:' AS [Info], COUNT(*) AS [Count] FROM [dbo].[LeadsClients]
UNION ALL
SELECT 'Interactions Count:', COUNT(*) FROM [dbo].[Interactions];
GO
