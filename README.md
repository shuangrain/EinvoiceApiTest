# 財政部電子發票 API 測試工具
這是我最近在串接財政部 API 時自己寫的測試工具，希望能讓大家在串接測試上更加方便。
<br />
歡迎 PR 財政部的 API 清單  <--  只是自己懶的新增 XDD

* 前端：Angular 4.0
* 後端：ASP.NET NetCore 2.0 - ASP.NET Web Api
* Live Demo：[https://exfast.me/einvoiceApiTest](https://exfast.me/einvoiceApiTest)
* SourceCode：[https://github.com/shuangrain/EinvoiceApiTest](https://github.com/shuangrain/EinvoiceApiTest)
* Blog：[https://blog.exfast.me/2017/12/sideproject-ministry-of-finance-e-invoicing-api-testing-tools/](https://blog.exfast.me/2017/12/sideproject-ministry-of-finance-e-invoicing-api-testing-tools/)

## 操作說明

* 環境變數

	1. 簽章 (Signature)：將 Request 參數按照名稱排序並以 HMAC-SHA1 加密後加在 Query 後方傳送至財政部
	2. 測試環境：將資料傳送至[財政部測試環境](https://wwwtest.einvoice.nat.gov.tw/)
	3. Client 模式：以另開新頁的方式呼叫財政部 API

* 選擇 Api
  <br />
  要測試的 API

* 財政部加密用 Key
  <br />
  財政部提供的 HMAC-SHA1 加密用 AppKey

* Api 位置
  <br />
  呼叫的 API 位置

* Request
  <br />
  傳送給財政部的資料

## 使用方法

1. 開啟 <code>/src/appsettings.json</code>
2. 將財政部提供的 <code>AppKey</code> 與 <code>AppID</code> 填上去
3. 用 Visual Studio 2017 開啟並執行即可 !!

## Api 新增

開啟 <code>/src/Json/ApiList.json</code> 且依照下面的參數新增 Api

#### 參數介紹

| 參數名稱 | 參數說明 |
|----------|--------------------------------------------------------------------------------|
| TypeName | Api 類型 (依此欄位排序) |
| ApiName | Api 名稱 |
| ApiUrl | Api 位置 |
| Param | Api 需傳送的參數範本 (參數 timeStamp, expTimeStamp, uuid, appID 會被自動取代) |
