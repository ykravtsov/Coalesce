
/// <reference path="../coalesce.dependencies.d.ts" />

// Knockout View Model for: CaseProduct
// Auto Generated by IntelliTect.Coalesce

module ViewModels {

	export class CaseProduct extends Coalesce.BaseViewModel<CaseProduct>
    {
        protected modelName = "CaseProduct";
        protected primaryKeyName = "caseProductId";
        protected modelDisplayName = "Case Product";

        protected apiController = "/CaseProduct";
        protected viewController = "/CaseProduct";
    
        /** 
            The enumeration of all possible values of this.dataSource.
        */
        public dataSources: typeof ListViewModels.CaseProductDataSources = ListViewModels.CaseProductDataSources;

        /**
            The data source on the server to use when retrieving the object.
            Valid values are in this.dataSources.
        */
        public dataSource: ListViewModels.CaseProductDataSources = ListViewModels.CaseProductDataSources.Default;

        /** Behavioral configuration for all instances of CaseProduct. Can be overidden on each instance via instance.coalesceConfig. */
        public static coalesceConfig: Coalesce.ViewModelConfiguration<CaseProduct>
            = new Coalesce.ViewModelConfiguration<CaseProduct>(Coalesce.GlobalConfiguration.viewModel);

        /** Behavioral configuration for the current CaseProduct instance. */
        public coalesceConfig: Coalesce.ViewModelConfiguration<CaseProduct>
            = new Coalesce.ViewModelConfiguration<CaseProduct>(CaseProduct.coalesceConfig);
    
        // Observables
        public caseProductId: KnockoutObservable<number> = ko.observable(null);
        public caseId: KnockoutObservable<number> = ko.observable(null);
        public case: KnockoutObservable<ViewModels.Case> = ko.observable(null);
        public productId: KnockoutObservable<number> = ko.observable(null);
        public product: KnockoutObservable<ViewModels.Product> = ko.observable(null);

       
        /** Display text for Case */
        public caseText: KnockoutComputed<string>;
        /** Display text for Product */
        public productText: KnockoutComputed<string>;
        


        /** Pops up a stock editor for object case */
        public showCaseEditor: (callback?: any) => void;
        /** Pops up a stock editor for object product */
        public showProductEditor: (callback?: any) => void;




        /** 
            Load the ViewModel object from the DTO. 
            @param force: Will override the check against isLoading that is done to prevent recursion. False is default.
            @param allowCollectionDeletes: Set true when entire collections are loaded. True is the default. In some cases only a partial collection is returned, set to false to only add/update collections.
        */
        public loadFromDto = (data: any, force: boolean = false, allowCollectionDeletes: boolean = true) => {
            if (!data || (!force && this.isLoading())) return;
            this.isLoading(true);
            // Set the ID 
            this.myId = data.caseProductId;
            this.caseProductId(data.caseProductId);
            // Load the lists of other objects
            // Objects are loaded first so that they are available when the IDs get loaded.
            // This handles the issue with populating select lists with correct data because we now have the object.
            if (!data.case) { 
                if (data.caseId != this.caseId()) {
                    this.case(null);
                }
            }else {
                if (!this.case()){
                    this.case(new Case(data.case, this));
                }else{
                    this.case().loadFromDto(data.case);
                }
                if (this.parent && this.parent.myId == this.case().myId && Coalesce.Utilities.getClassName(this.parent) == Coalesce.Utilities.getClassName(this.case()))
                {
                    this.parent.loadFromDto(data.case, undefined, false);
                }
            }
            if (!data.product) { 
                if (data.productId != this.productId()) {
                    this.product(null);
                }
            }else {
                if (!this.product()){
                    this.product(new Product(data.product, this));
                }else{
                    this.product().loadFromDto(data.product);
                }
                if (this.parent && this.parent.myId == this.product().myId && Coalesce.Utilities.getClassName(this.parent) == Coalesce.Utilities.getClassName(this.product()))
                {
                    this.parent.loadFromDto(data.product, undefined, false);
                }
            }

            // The rest of the objects are loaded now.
            this.caseId(data.caseId);
            this.productId(data.productId);
            if (this.coalesceConfig.onLoadFromDto()){
                this.coalesceConfig.onLoadFromDto()(this as any);
            }
            this.isLoading(false);
            this.isDirty(false);
            this.validate();
        };

        /** Save the object into a DTO */
        public saveToDto = (): any => {
            var dto: any = {};
            dto.caseProductId = this.caseProductId();

            dto.caseId = this.caseId();
            if (!dto.caseId && this.case()) {
                dto.caseId = this.case().caseKey();
            }
            dto.productId = this.productId();
            if (!dto.productId && this.product()) {
                dto.productId = this.product().productId();
            }

            return dto;
        }


        constructor(newItem?: any, parent?: any){
            super();
            var self = this;
            self.parent = parent;
            self.myId;
            
            ko.validation.init({
                grouping: {
                    deep: true,
                    live: true,
                    observable: true
                }
            });

            // SetupValidation {
			self.caseId = self.caseId.extend({ required: {params: true, message: "Case is required."} });
			self.productId = self.productId.extend({ required: {params: true, message: "Product is required."} });
            
            self.errors = ko.validation.group([
                self.caseProductId,
                self.caseId,
                self.case,
                self.productId,
                self.product,
            ]);
            self.warnings = ko.validation.group([
            ]);

            // Computed Observable for edit URL
            self.editUrl = ko.computed(function() {
                return self.coalesceConfig.baseViewUrl() + self.viewController + "/CreateEdit?id=" + self.caseProductId();
            });

            // Create computeds for display for objects
			self.caseText = ko.computed(function()
			{   // If the object exists, use the text value. Otherwise show 'None'
				if (self.case() && self.case().caseKey()) {
					return self.case().caseKey().toString();
				} else {
					return "None";
				}
			});
			self.productText = ko.computed(function()
			{   // If the object exists, use the text value. Otherwise show 'None'
				if (self.product() && self.product().name()) {
					return self.product().name().toString();
				} else {
					return "None";
				}
			});

    


            self.showCaseEditor = function(callback: any) {
                if (!self.case()) {
                    self.case(new Case());
                }
                self.case().showEditor(callback)
            };
            self.showProductEditor = function(callback: any) {
                if (!self.product()) {
                    self.product(new Product());
                }
                self.product().showEditor(callback)
            };

            // Load all child objects that are not loaded.
            self.loadChildren = function(callback) {
                var loadingCount = 0;
                // See if self.case needs to be loaded.
                if (self.case() == null && self.caseId() != null){
                    loadingCount++;
                    var caseObj = new Case();
                    caseObj.load(self.caseId(), function() {
                        loadingCount--;
                        self.case(caseObj);
                        if (loadingCount == 0 && $.isFunction(callback)){
                            callback();
                        }
                    });
                }
                // See if self.product needs to be loaded.
                if (self.product() == null && self.productId() != null){
                    loadingCount++;
                    var productObj = new Product();
                    productObj.load(self.productId(), function() {
                        loadingCount--;
                        self.product(productObj);
                        if (loadingCount == 0 && $.isFunction(callback)){
                            callback();
                        }
                    });
                }
                if (loadingCount == 0 && $.isFunction(callback)){
                    callback();
                }
            };

            // This stuff needs to be done after everything else is set up.
            // Complex Type Observables

            self.caseId.subscribe(self.autoSave);
            self.case.subscribe(self.autoSave);
            self.productId.subscribe(self.autoSave);
            self.product.subscribe(self.autoSave);
        
            if (newItem) {
                if ($.isNumeric(newItem)) self.load(newItem);
                else self.loadFromDto(newItem, true);
            }
        }
    }





    export namespace CaseProduct {

        // Classes for use in method calls to support data binding for input for arguments
    }
}