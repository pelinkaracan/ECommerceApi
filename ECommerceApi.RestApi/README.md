## HOW CAN COMPILE AND RUN  ECOMMERCEWEB PAGE
- ### Requirements
Before begin, make sure you have the following prerequisites installed:

- .NET SDK
- MongoDB
- Visual Studio 2022
- ### Setup
### Mongo Db Restore
1. Firstly, we have to restore our database so please download this tools from this link; <https://www.mongodb.com/try/download/database-tools> 

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/a86363f8-d60b-4023-828c-c6717eb91655)

2. After download, you should go to folder that your downloaded.Double click and start the download.You can click next button  until see install button then you can click install.
3. Now, we can to this path and copy: 

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/df291276-5395-4b7e-8112-f848410eb869)

4. You can add path to advance system settings. You should that because you have to run restore command in command prompt.

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/83623ba5-171e-4e48-8697-707f5cba0c8c)

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/2ca0badf-d2f6-4034-965b-105818feceaa)

Add this path : C:\Program Files\MongoDB\Tools\100\bin.

5. Open command prompt, go your database backup folder and write this command "mongorestore –db ECommerceWebPageApiDb"

Note: Backup folder should be  in this path:  “..\ECommerceWebPage\MongoDbBackUp\dump”
### How can you run Rest Api?
1. Open this folder in the folder I sent you, and double click .sln file and open with visual studio 2022;

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/ff0d3361-a8cd-415e-b007-7a6b06c4244a)

2. Right click project and choose the open in terminal.

![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/25024c71-47c1-42d8-a65b-185dcd1289a7)

3. Write “dotnet run” in terminal and click enter.
4. Your rest api host in <https://localhost:7060/> and you can see this page that is in below picture.
![image](https://github.com/pelinkaracan/ECommerceApi/assets/11795893/c2881aeb-e694-4273-a4ab-98c442437446)
