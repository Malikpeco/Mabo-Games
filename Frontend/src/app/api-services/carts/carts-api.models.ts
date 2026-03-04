export interface AddToCartCommand{
    gameId:number;
}

export interface CartItemDto{
    id:number;
    gameId:number;
    coverImageURL?:string;
    gameName:string;
    price:number;
    addedAt:string;
    isSaved:boolean;
}

export interface CartDto{
    id:number;
    cartItems:CartItemDto[];
    totalPrice:number;
}

export interface SwitchItemStateCommand{
    cartItemId:number;
}