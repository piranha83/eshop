export interface Order {
  name: string;
  phone: string;
  address: { 
    street: string, 
    city: string, 
    state: string, 
    zip: string
  },
  delivery: "athome" | "atshop",
  checkout: "cash" | "paypal"
}
