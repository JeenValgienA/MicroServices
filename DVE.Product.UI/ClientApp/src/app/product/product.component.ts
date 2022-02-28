import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProductDetail, ProductName } from '../model/product';
import { ApiService } from '../services/api.service';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
    selector: 'product-details',
    templateUrl: './product.component.html'
})
export class ProductComponent {
    public productNames: ProductName[];
    public productDetail: ProductDetail;
    id: string;
    productId: number;
    public sourceAction: number = 0; //1-AllProduct, 2-ProductDetail

    constructor(
        private api: ApiService,
        private _activatedRoute: ActivatedRoute
    ) { }

    ngOnInit() {
        this._activatedRoute.params.forEach((params: Params) => {
            this.id = params['id']; // get the id from url
            if (Number.parseInt(this.id, this.productId) > 0) {

                this.api.getProductById(this.id).subscribe(res => {
                    this.productDetail = res;
                    this.sourceAction = 2;
                })
            }
            else {
                this.api.getProductNames().subscribe(res => {
                    this.productNames = res;
                    this.sourceAction = 1;
                })
            }
        });

    }
}