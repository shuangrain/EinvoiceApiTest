import { Component, Inject } from '@angular/core';
import { Http, RequestOptionsArgs, RequestOptions, RequestMethod } from '@angular/http';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    public apiList: ApiList[];
    public apiRequest: string = "";
    public apiResponse: string = "";
    public apiUrl: string = "";
    public apiKey: string = "";
    public isNeedSignature: boolean = true;
    public isTestMode: boolean = false;
    public isClientMode: boolean = false;

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string) {
        http.get(baseUrl + 'api/Einvoice').subscribe(result => {
            const response = result.json() as ApiResult;

            if (response.rtnCode === 1) {
                this.apiKey = response.rtnData.appKey;
                this.apiList = response.rtnData.apiList as ApiList[];

                if (this.apiList && this.apiList.length > 0) {
                    this.changeApi(this.apiList[0].id);
                }
            }
        }, error => console.error(error));
    }

    public changeApi(id: number) {
        let api = this.apiList.find((item) => item.id == id);
        if (api) {
            this.apiRequest = JSON.stringify(api.param, null, "\t")
            this.apiUrl = api.apiUrl;
            this.apiResponse = "";
        }
    }

    public callApi(form: NgForm) {
        this.apiResponse = "";

        let formData: FormData = new FormData();
        for (var key in form.value) {
            formData.append(key, form.value[key]);
        }

        this.http.post(this.baseUrl + 'api/Einvoice', formData).subscribe(result => {
            const response = result.json() as ApiResult;

            if (response.rtnCode === 100) {
                window.open(response.rtnData);
            } else if (response.rtnCode === 1) {
                try {
                    response.rtnData.result = JSON.parse(response.rtnData.result);
                    this.apiResponse = JSON.stringify(response.rtnData, null, "\t");
                } catch (e) {
                    this.apiResponse = (response.rtnData as string);
                }
            } else {
                this.apiResponse = JSON.stringify(response.rtnData, null, "\t");

                if (this.isClientMode) {
                    alert(this.apiResponse);
                }
            }
        }, error => console.error(error));
    }
}

interface ApiList {
    id: number;
    typeName: string;
    apiName: string;
    apiUrl: string;
    param: any;
}

interface ApiResult {
    rtnCode: number;
    rtnData: any;
}