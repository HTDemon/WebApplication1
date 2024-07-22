# 面試專案說明

前端：React Creat App(RCA), material-ui  
後端：ASP.NET(8.0) Core 建立 Minimal API並支援 OpenAPI 標準  
資料庫：MongoDB
網站執行埠號：5017

RCA npm run build產出的內容需要複製到frontend資料夾中作為靜態檔案  

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

Program.cs中有產生基礎資料到DB中的方法
```
if (!db.Patient.Any())
{
    db.Patient.AddRange(new[] {
        new Patient { Id = 1, Name = "王承恩", OrderId = 1 },
        new Patient { Id = 2, Name = "陳品妍", OrderId = 2 },
        new Patient { Id = 3, Name = "李宥廷", OrderId = 3 },
        new Patient { Id = 4, Name = "張苡菲", OrderId = 4 },
        new Patient { Id = 5, Name = "吳子晴", OrderId = 5 },
    });
    db.Order.AddRange(new[] {
        new Order { Id = 1, Message = "This patient was put into the regimen of pain control with PCA." },
        new Order { Id = 2, Message = "D.C. all regular narcotics." },
        new Order { Id = 3, Message = "Oxygen breathing equipment has to be standby for urgency." },
        new Order { Id = 4, Message = "PCA dose: 2.0mg." },
        new Order { Id = 5, Message = "Naloxone(Narcan) 0.4mg/ml amp should be ready at bedside." },
    });
    db.SaveChanges();
}
```

### 主畫面
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_1.gif)


### 醫囑Dialog，編輯醫囑
---
編輯後會關閉Dialog有點不方便  
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_2.gif)


### 新增醫囑
---
新增一筆後會將Id更新回Patient.OrderId  
新增後會關閉Dialog有點不方便  
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_3.gif)


### OpenAPI(Swagger UI)測試頁
---
![image](https://github.com/HTDemon/WebApplication1/blob/master/WebApplication1/README/IE_4.gif)

### 專案未來展望
---
1.讓新增/編輯醫囑後不用關閉醫囑Dialog  
2.前端新增/編輯加入正規化表示  
3.建置後端專案的時候能順便建置前端專案並將其產出放入frontend資料夾中 
4.將專案裝進Docker中  
