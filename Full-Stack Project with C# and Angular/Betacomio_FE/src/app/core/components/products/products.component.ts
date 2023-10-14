import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginComponent } from 'src/app/features/login/login.component';
import { ColorSize } from 'src/app/models/colorSize';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { CartManagerService } from 'src/app/services/cart-manager.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent {

  modelName: string = '';
  colorSize: ColorSize | null = null;  

  constructor(private route: ActivatedRoute,
              public srv: ApiManagerService,
              private csrv: CartManagerService,
              public dialog: MatDialog) {
    this.modelName = this.route.snapshot.params['modelName']
    this.srv.getProducts(this.modelName)
    console.log(this.modelName)
  }

  addToCart(f: NgForm) {
    if (sessionStorage.length > 0) {
      this.colorSize = f.value;
      this.srv.products.forEach( product => {
        if (product.color === this.colorSize?.color && product.size === this.colorSize?.size) {
          this.csrv.toCart.push(product);
          } else if (product.size === null && product.color === null) {
            this.csrv.toCart.push(product);
          } else if (product.color === this.colorSize?.color && product.size === null) {
            this.csrv.toCart.push(product);
          } else if (product.color === null && product.size === this.colorSize?.size) {
            this.csrv.toCart.push(product);
          }
      });  
      this.csrv.addToSession() ;
    } else {
      this.dialog.open(LoginComponent)
        .afterClosed()
          .pipe(                                  //afterClosed vuole un Observable, quindi utilizziamo .pipe per effettuare operazioni al suo interno, richiamo 
            tap(() => {                           // addToCart e completo la subscribe, il metodo riparte entra nel primo if
              this.addToCart(f);
            })).subscribe();
    }    
  }

  selectWeight(size: any) {
    
    this.srv.products.forEach( product => {
      if (size === product.size) {
        this.srv.weight = product.weight
      }      
    }) 
  }
}