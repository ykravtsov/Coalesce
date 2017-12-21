
/// <reference path="../coalesce.dependencies.d.ts" />

// Knockout List View Model for: Product
// Generated by IntelliTect.Coalesce

module ListViewModels {

    export namespace ProductDataSources {
        export class Default extends Coalesce.DataSource<ViewModels.Product> { }
            }

    export class ProductList extends Coalesce.BaseListViewModel<ViewModels.Product> {
        protected modelName = "Product";
        protected apiController = "/Product";
        public modelKeyName = "productId";
        public itemClass = ViewModels.Product;

        public filter: {
            productId?:string;
            name?:string;
        } = null;
    
        /** 
            The namespace containing all possible values of this.dataSource.
        */
        public dataSources: typeof ProductDataSources = ProductDataSources;

        /**
            The data source on the server to use when retrieving objects.
            Valid values are in this.dataSources.
        */
        public dataSource: Coalesce.DataSource<ViewModels.Product> = new this.dataSources.Default();

        public static coalesceConfig = new Coalesce.ListViewModelConfiguration<ProductList, ViewModels.Product>(Coalesce.GlobalConfiguration.listViewModel);
        public coalesceConfig: Coalesce.ListViewModelConfiguration<ProductList, ViewModels.Product>
            = new Coalesce.ListViewModelConfiguration<ProductList, ViewModels.Product>(ProductList.coalesceConfig);


        protected createItem = (newItem?: any, parent?: any) => new ViewModels.Product(newItem, parent);

        constructor() {
            super();
        }
    }

    export namespace ProductList {
        // Classes for use in method calls to support data binding for input for arguments
    }
}