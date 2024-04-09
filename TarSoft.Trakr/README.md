# TarSoft.Trakr
Use command to create db:

docker run -d `
  --name postgresstest `
  -e POSTGRES_DB=postgresstest `
  -e POSTGRES_USER=your_user `
  -e POSTGRES_PASSWORD=YourStrong!Passw0rd `
  -p 5432:5432 `
  postgres


  Update-database