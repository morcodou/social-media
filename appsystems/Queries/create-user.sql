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
