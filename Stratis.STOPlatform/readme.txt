### add migration script
add-migration <name> -project Stratis.STOPlatform.Data -startup Stratis.STOPlatform

### apply migrations to database
update-database -project Stratis.STOPlatform.Data -startup Stratis.STOPlatform
