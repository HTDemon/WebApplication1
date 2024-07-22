# 面試專案說明
---
請將appsettings.json中的mongodb{ip}及{port}替換成自己的DB ip及port。
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDatabase": {
    "ConnectionString": "mongodb://{ip}:{port}",
    "DatabaseName": "main",
    "BooksCollectionName": "Books"
  },
  "AllowOrigins": [
    "http://localhost:3000"
  ]
}
```

### 主畫面
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_1.gif)

### 醫囑Dialog，編輯醫囑
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_2.gif)

### 新增醫囑
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_3.gif)

### OpenAPI(Swagger UI)測試頁
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_4.gif)
