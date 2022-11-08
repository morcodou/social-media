coverlet SocialMedia.Post.Test/Post.Command/Post.Command.Infrastructure.Test/bin/Debug/net6.0/Post.Command.Infrastructure.Test.dll --target "dotnet" --targetargs "test --no-build"

/* Change to the SocialMedia database */
USE SocialMedia;
GO

/* Create user */
IF NOT EXISTS(SELECT *
FROM sys.server_principals
WHERE name = 'smuser')
BEGIN
	CREATE LOGIN smuser WITH PASSWORD=N'SmPA$$06500', DEFAULT_DATABASE=SocialMedia
END


IF NOT EXISTS(SELECT *
FROM sys.database_principals
WHERE name = 'smuser')
BEGIN
	EXEC sp_adduser 'smuser', 'smuser', 'db_owner';
END
