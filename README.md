### add migration script
add-migration <name> -project Stratis.ICOPlatform.Data -startup Stratis.ICOPlatform

### apply migrations to database
update-database -project Stratis.ICOPlatform.Data -startup Stratis.ICOPlatform
