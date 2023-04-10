using ADEntities.Queries;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Convert = ADEntities.Converts;

namespace ADSystem.Manager
{
    public class SalesOnLineManager
    {
        public SalesOnLine _salesOnLine = new SalesOnLine();
        public Products _products = new Products();
        public Convert.SalesOnLine _convertSalesOnLine = new Convert.SalesOnLine();
        private const int CANCELEDSALE = 6;

        public SaleOnLineViewModel Get(int id)
        {
            return _salesOnLine.Get(id);
        }
        public List<SaleOnLineViewModel> GetBySellerId(int id)
        {
            return _salesOnLine.GetBySellerId(id);
        }
        public Tuple<int, List<SaleOnLineViewModel>> GetAll(DateTime start, DateTime end, int status, string customer, string user, int page, int pageSize)
        {
            var result = _salesOnLine.GetAll(start, end, status, customer, user, page, pageSize);

            return Tuple.Create(result.Item1, _convertSalesOnLine.EntitiesToList(result.Item2));
        }
        public SaleOnLineViewModel Update(SaleOnLineViewModel sale)
        {
            var entity = _convertSalesOnLine.PrepareUpdate(sale);
            var result = _salesOnLine.Update(entity);

            return _convertSalesOnLine.TableToModel(result);
        }
        public SaleOnLineViewModel UpdateStatus(int id, int status, int? branch)
        {
            var result = _convertSalesOnLine.TableToModel(_salesOnLine.UpdateStatus(id, status, branch));

            if(status == CANCELEDSALE)
            {
                var sale = Get(id);

                foreach(var product in sale.Productos)
                {
                    foreach (var dist in product.Distribucion)
                    {
                        _products.UpdateProductBranchExistStock((int)product.IdProducto, dist.IdSucursal, (int)dist.Cantidad);
                    }                        
                }   
            }

            return result;
        }

        public SaleOnLineViewModel UpdateBill(int id, string bill)
        {
            var result = _convertSalesOnLine.TableToModel(_salesOnLine.UpdateBill(id, bill));
            
            return result;
        }

        public SaleOnLineViewModel UpdateAssignedUser(int id, int idUser)
        {
            var result = _convertSalesOnLine.TableToModel(_salesOnLine.UpdateAssignedUser(id, idUser));

            return result;
        }

        public SaleOnLineViewModel UpdateSendingData(int id, string sendingProvider, string guideNumber)
        {
            var result = _convertSalesOnLine.TableToModel(_salesOnLine.UpdateSendingData(id, sendingProvider, guideNumber));

            return result;
        }
    }
}