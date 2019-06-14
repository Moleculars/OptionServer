using Bb.Option.Printings;
using Bb.Option.Validators;
using Bb.OptionService.Models;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Option.Commands
{
    public static partial class Command
    {

        public static CommandLineApplication CommandType(this CommandLineApplication app)
        {

            var cmd = app.Command("type", config =>
            {

                config.Description = "create, update, list, ... type of configuration";
                config.HelpOption(HelpFlag);

            });

            cmd.Command("add", config =>
            {

                config.Description = "Add a new type of configuration";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argTypeName = validator.Argument("typeName",
                    "type name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argExtension = validator.Argument("extension",
                    "extension (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argContractFile = validator.Option("-fileContract",
                    "file that contains contract validator"
                    , ValidatorExtension.EvaluateFileExist
                );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    var contract = argContractFile.HasValue()
                        ? Helper.LoadContentFromFile(argContractFile.Value())
                        : string.Empty
                        ;

                    contract = contract.SerializeContract();

                    var model = new TypeModel()
                    {
                        Groupname = Helper.Parameters.WorkingGroup,
                        TypeName = argTypeName.Value,
                        Extension = argExtension.Value,
                        Validator = contract,
                    };

                    var result = Client.Post<RootResultModel<TypeModel>>("api/type/add", model, GetToken());
                    result.Wait();

                    Output.WriteLine($"type {argTypeName.Value} is added on group {result.Result.Datas.Groupname}.");

                    return 0;

                });
            });

            cmd.Command("list", config =>
            {

                config.Description = "list all types of configuration for working group is setted";
                config.HelpOption(HelpFlag);

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    var result = Client.Get<RootResultModel<List<TypeModel>>>($"api/type/list/{Helper.Parameters.WorkingGroup}", GetToken());
                    result.Wait();

                    var _result = Helper.ConvertDataToShow(result.Result.Datas);

                    PrintDataExtensions.ExtendedASCIIBorder();
                    ConvertToDatatable
                        .ConvertList(_result, "types")
                        .Print();

                    return 0;

                });
            });

            cmd.Command("listext", config =>
            {

                config.Description = "list all types of configuration when extension is equal for working group is setted";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argTypeName = validator.Argument("extension",
                    "extension of the file that contains the configuration content (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    var result = Client.Get<RootResultModel<List<TypeModel>>>($"api/type/extension/{Helper.Parameters.WorkingGroup}/{argTypeName.Value}", GetToken());
                    result.Wait();

                    var _result = Helper.ConvertDataToShow(result.Result.Datas);

                    PrintDataExtensions.ExtendedASCIIBorder();
                    ConvertToDatatable
                        .ConvertList(_result, "types")
                        .Print();

                    return 0;

                });
            });

            cmd.Command("get", config =>
            {

                config.Description = "return informations of specified type of configuration for working group is setted";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argTypeName = validator.Argument("typeName",
                    "type name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var _contract = validator.OptionNoValue("-contract",
                    "want to show contract validation"
                    );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    var result = Client.Get<RootResultModel<TypeModel>>($"api/type/get/{Helper.Parameters.WorkingGroup}/{argTypeName.Value}", GetToken());
                    result.Wait();

                    var type = result.Result.Datas;

                    if (_contract.HasValue())
                    {
                        Output.WriteLine($"{type.TypeName}, Version {type.Version}");
                        Output.WriteLine(type.Validator.DeserializeContract());
                    }
                    else
                    {

                        PrintDataExtensions.ClearBorder();
                        ConvertToDatatable
                            .Convert(type, "type")
                            .PrintList();

                    }
                    return 0;

                });
            });

            cmd.Command("extension", config =>
            {

                config.Description = "change extension of file that will contains configuration";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argTypeName = validator.Argument("typeName",
                    "type name (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                var argExtension = validator.Argument("extension",
                    "extension of file. (this argument is required)"
                    , ValidatorExtension.EvaluateRequired
                );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    var model = new TypeModel()
                    {
                        Groupname = Helper.Parameters.WorkingGroup,
                        TypeName = argTypeName.Value,
                        Extension = argExtension.Value,
                    };

                    var result = Client.Post<RootResultModel<TypeModel>>("api/type/updateExtension", model, GetToken());
                    result.Wait();

                    Output.WriteLine($"extension of type {argTypeName.Value} in group {result.Result.Datas.Groupname} is changed to {argExtension.Value}.");

                    return 0;

                });
            });

            cmd.Command("contract", config =>
            {

                config.Description = "update contract. Becarefull, if you don't specify any option for contract, it is setted to emptys";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config, false);

                var argTypeName = validator.Argument("typeName",
                    "type name (this argument is required)."
                    , ValidatorExtension.EvaluateName
                    , ValidatorExtension.EvaluateRequired
                );

                var argContractFile = validator.Option("-file",
                    "file that contains contract validator"
                    , ValidatorExtension.EvaluateFileExist
                );

                var argContractText = validator.Option("-text",
                    "text that contains contract validator"
                );

                var argContractText64 = validator.Option("-text64",
                    "text encoded in base64 that contains contract validator"
                );

                config.OnExecute(() =>
                {

                    var r = ValidatorExtension.EvaluateGroupName();
                    if (r > 0)
                        return r;

                    if (validator.Evaluate() > 0)
                        return 2;

                    string contract = string.Empty;

                    if (argContractFile.HasValue())
                        contract = Helper.LoadContentFromFile(argContractFile.Value());

                    else if (argContractText.HasValue())
                        contract = argContractText.Value();

                    else if (argContractText64.HasValue())
                    {

                        contract = argContractText64.Value();
                        byte[] array = new byte[0];
                        try
                        {
                            array = Convert.FromBase64String(contract);
                        }
                        catch (System.Exception)
                        {
                            Output.ErrorWriteLine("contrat text is not valid base 64 text");
                            return 1;
                        }

                        try
                        {
                            contract = Helper.LoadContentFromText(array);
                        }
                        catch (Exception)
                        {
                            Output.ErrorWriteLine("base 64 text is not valid encoded text");
                            return 1;
                        }

                    }

                    var model = new TypeModel()
                    {
                        Groupname = Helper.Parameters.WorkingGroup,
                        TypeName = argTypeName.Value,
                        Validator = contract.SerializeContract(),
                    };

                    var result = Client.Post<RootResultModel<EnviromnentModel>>("api/type/updateContract", model, GetToken());
                    result.Wait();

                    Output.WriteLine($"contract of type {argTypeName.Value} is changed in group {result.Result.Datas.Groupname}.");

                    PrintDataExtensions.ClearBorder();
                    ConvertToDatatable
                        .Convert(result.Result.Datas, "type")
                        .PrintList();

                    return 0;

                });
            });

            return app;

        }

    }

    public class TypeToShow
    {

        public TypeToShow(TypeModel type)
        {
            Group = type.Groupname;
            Type = type.TypeName;
            Extension = type.Extension;
            Version = type.Version;

            if (!string.IsNullOrEmpty(type.Validator))
            {
                Sha256 = type.Sha256;
                var t = type.Validator.DeserializeContract().Replace("\r", "").Replace("\n", "").Take(15).ToArray();
                Contract = string.Concat(t) + " ...";
            }

        }

        public string Group { get; }

        public string Type { get; }

        public string Extension { get; }

        public int Version { get; }

        public string Sha256 { get; }
        public string Contract { get; }
    }

}
