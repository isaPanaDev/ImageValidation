using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ImageValidation.Service.Data;
using ImageValidation.Core;
using System.Data;

namespace ImageValidation.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IImageValidationService
    {
        //Validator Collector Tool
        #region Validator Collector Tool

        [OperationContract]
        Users ValidateUser(string Username, string Password);

        [OperationContract]
        bool SaveAccountHistory(Users users);


        [OperationContract]
        int CheckUsernameExists(Users ObjUser);

        [OperationContract]
        bool SaveFailedLogon(Users ObjUsers);


        [OperationContract]
        bool SaveComputerInformation(Computer ObjComp, List<Applications> ObjAppsLst, List<HotFix> ObjDriverLst);

        [OperationContract]
        int SaveComputerInformationFromXml(string xmlData);

        [OperationContract]
        bool CheckModelRecordExists(string Model);

        [OperationContract]
        int SaveComputerInfoUpdateCheckedModelRecord(string xmlData);

        [OperationContract]
        int SaveComputerInfoUpdateCheckedModelRecordSet1(string xmlData);

        [OperationContract]
        bool CheckProductRecordExists(string Model);

        [OperationContract]
        int SaveComputerInfoUpdateCheckedProductRecord(string xmlData);

        [OperationContract]
        int SaveComputerInfoUpdateCheckedProductRecordSet1(string xmlData);

        [OperationContract]
        int SaveComputerInfoUpdateCheckedProductModelRecord(string xmlData);

        #endregion
        //ImageValidationClient Tool..
        #region Client Collection tool

        [OperationContract]
        int SeleteComputerInfoByModel(string model);

        [OperationContract]
        int SeleteComputerInfoByProduct(string product);

        [OperationContract]
        string SelectApplicationsByComputerID(int computerId);

        [OperationContract]
        string SelectDriverByComputerID(int computerId);

        [OperationContract]
        string SelectHotFixByComputerID(int computerId);

        [OperationContract]
        string SelectFileFolderByComputerID(int computerId);

        [OperationContract]
        string SelectRegistryByComputerID(int computerId);



        #endregion


    }
}
