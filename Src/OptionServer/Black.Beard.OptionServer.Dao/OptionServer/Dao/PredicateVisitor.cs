using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bb.OptionServer
{

    internal class ConstantVisitor : ExpressionVisitor
    {
        private object _value;

        private ConstantVisitor()
        {

        }

        public static object GetConstant(Expression e)
        {
            var visitor = new ConstantVisitor();
            visitor.Visit(e);
            return visitor._value;
        }


        protected override Expression VisitBlock(BlockExpression node)
        {
            Stop();
            return base.VisitBlock(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            Stop();
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Stop();
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {

            _value = node.Value;
            return node;

        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            Stop();
            return base.VisitDefault(node);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            Stop();
            return base.VisitDynamic(node);
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            Stop();
            return base.VisitElementInit(node);
        }

        protected override Expression VisitExtension(Expression node)
        {
            Stop();
            return base.VisitExtension(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            Stop();
            return base.VisitGoto(node);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            Stop();
            return base.VisitIndex(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            Stop();
            return base.VisitInvocation(node);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            Stop();
            return base.VisitLabel(node);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            Stop();
            return base.VisitLabelTarget(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            Stop();
            return base.VisitListInit(node);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            Stop();
            return base.VisitLoop(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {

            base.VisitMember(node);

            if (_value != null)
            {
                if (node.Member is FieldInfo f)
                    _value = f.GetValue(_value);

                else if (node.Member is PropertyInfo p)
                    _value = p.GetValue(_value);
            }

            return node;

        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            Stop();
            return base.VisitMemberAssignment(node);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Stop();
            return base.VisitMemberBinding(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            Stop();
            return base.VisitMemberInit(node);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            Stop();
            return base.VisitMemberListBinding(node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            Stop();
            return base.VisitMemberMemberBinding(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Stop();
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            Stop();
            return base.VisitNew(node);
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            Stop();
            return base.VisitNewArray(node);
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            Stop();
            return base.VisitRuntimeVariables(node);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            Stop();
            return base.VisitSwitch(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            Stop();
            return base.VisitTry(node);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            Stop();
            return base.VisitSwitchCase(node);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Stop();
            return base.VisitTypeBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Diagnostics.DebuggerStepThrough]
        private void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
                return;
            }

            throw new NotImplementedException();

        }


    }


    internal class PredicateVisitor : ExpressionVisitor
    {

        public PredicateVisitor(StringBuilder sb, List<DbParameter> arguments, IQueryPredicateGenerator generator, ObjectMapping mapping, SqlManager manager)
        {
            _sb = sb;
            _arguments = arguments;
            _generator = generator;
            _mapping = mapping;
            _manager = manager;
        }


        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {

            switch (node.NodeType)
            {

                case ExpressionType.Equal:

                    Visit(node.Left);

                    switch (node.Method.Name)
                    {

                        case "op_Equality":
                            _sb.Append(_generator.WriteEquality());
                            break;

                        default:
                            Stop();
                            break;
                    }

                    Visit(node.Right);

                    break;

                case ExpressionType.And:
                case ExpressionType.AndAlso:

                    _sb.Append("(");
                    Visit(node.Left);
                    _sb.Append(") AND (");
                    Visit(node.Right);
                    _sb.Append(")");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _sb.Append("(");
                    Visit(node.Left);
                    _sb.Append(") OR (");
                    Visit(node.Right);
                    _sb.Append(")");
                    break;

                default:
                    Stop();
                    break;

            }

            return node;

        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            Stop();
            return base.VisitBlock(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            Stop();
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Stop();
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {

            return base.VisitConstant(node);

            //object value = node.Value;
            //var varName = $"var{_arguments.Count}";

            //if (node.Type.IsClass && node.Type != typeof(string))
            //{
            //    var f = node.Type.GetFields()[0];
            //    value = f.GetValue(node.Value);
            //    varName = f.Name;

            //    AddParameter(value, varName);

            //}

            //return Expression.Constant(value);

        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            Stop();
            return base.VisitDefault(node);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            Stop();
            return base.VisitDynamic(node);
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            Stop();
            return base.VisitElementInit(node);
        }

        protected override Expression VisitExtension(Expression node)
        {
            Stop();
            return base.VisitExtension(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            Stop();
            return base.VisitGoto(node);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            Stop();
            return base.VisitIndex(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            Stop();
            return base.VisitInvocation(node);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            Stop();
            return base.VisitLabel(node);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            Stop();
            return base.VisitLabelTarget(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            Stop();
            return base.VisitListInit(node);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            Stop();
            return base.VisitLoop(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {

            Expression value = base.Visit(node.Expression);

            if (node.Member is PropertyInfo p)
            {

                if (value is ParameterExpression)
                {
                    var _name = _mapping.IndexByName[p.Name].FieldName;
                    _sb.Append(_generator.WriteMember(_name));
                    return node;
                }

                if (value.NodeType == ExpressionType.Constant)
                {
                    var _value = (value as ConstantExpression).Value;
                    _value = p.GetValue(_value);
                    AddParameter(value, p.Name);
                }
                else
                {
                    Stop();
                }

            }
            else if (node.Member is FieldInfo f)
            {

                if (value is ParameterExpression)
                {
                    var _name = _mapping.IndexByName[f.Name].FieldName;
                    _sb.Append(_generator.WriteMember(_name));
                    return node;
                }

                if (value.NodeType == ExpressionType.Constant)
                {
                    var _value = (value as ConstantExpression).Value;
                    _value = f.GetValue(_value);
                    AddParameter(_value, f.Name);
                }
                else
                {
                    Stop();
                }

            }

            return node;

        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            Stop();
            return base.VisitMemberAssignment(node);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Stop();
            return base.VisitMemberBinding(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            Stop();
            return base.VisitMemberInit(node);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            Stop();
            return base.VisitMemberListBinding(node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            Stop();
            return base.VisitMemberMemberBinding(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Stop();
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            Stop();
            return base.VisitNew(node);
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            Stop();
            return base.VisitNewArray(node);
        }

        //protected override Expression VisitParameter(ParameterExpression node)
        //{
        //    Stop();
        //    return base.VisitParameter(node);
        //}

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            Stop();
            return base.VisitRuntimeVariables(node);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            Stop();
            return base.VisitSwitch(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            Stop();
            return base.VisitTry(node);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            Stop();
            return base.VisitSwitchCase(node);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Stop();
            return base.VisitTypeBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {

            switch (node.NodeType)
            {

                case ExpressionType.Convert:
                    if (node.Method == null)
                        return node.Operand;

                    switch (node.Method.Name)
                    {

                        // do nothing
                        case "op_Implicit":
                            var result = Visit(node.Operand);
                            if (result is ConstantExpression c)
                            {

                                Stop();

                                if (c.Type == node.Type)
                                    return result;

                                var value = c.Value;
                                value = node.Method.Invoke(null, new object[] { value });
                                return Expression.Constant(value);

                            }

                            return node;

                        default:
                            Stop();
                            break;

                    }


                    break;

                default:
                    Stop();
                    break;
            }


            Stop();

            return base.VisitUnary(node);

        }


        private DbParameter AddParameter(object value, string varName)
        {
            DbParameter result = _arguments.FirstOrDefault(c => c.ParameterName == varName);

            if (result != null && result.Value != value)
            {
                varName = varName + _arguments.Count + 1;
                result = null;
            }

            if (result == null)
            {
                switch (value)
                {

                    case Guid t1:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Guid, value);
                        break;

                    case string t2:
                        result = _manager.CreateParameter(varName, System.Data.DbType.String, value);
                        break;

                    case bool t3:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Boolean, value);
                        break;

                    case byte t4:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Byte, value);
                        break;

                    case DateTimeOffset t5:
                        result = _manager.CreateParameter(varName, System.Data.DbType.DateTimeOffset, value);
                        break;

                    case decimal t6:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Decimal, value);
                        break;

                    case double t7:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Double, value);
                        break;

                    case Int16 t8:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Int16, value);
                        break;

                    case Int32 t9:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Int32, value);
                        break;

                    case Int64 t10:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Int64, value);
                        break;

                    case sbyte t11:
                        result = _manager.CreateParameter(varName, System.Data.DbType.SByte, value);
                        break;

                    case Single t12:
                        result = _manager.CreateParameter(varName, System.Data.DbType.Single, value);
                        break;

                    case UInt16 t13:
                        result = _manager.CreateParameter(varName, System.Data.DbType.UInt16, value);
                        break;

                    case UInt32 t14:
                        result = _manager.CreateParameter(varName, System.Data.DbType.UInt32, value);
                        break;

                    case UInt64 t15:
                        result = _manager.CreateParameter(varName, System.Data.DbType.UInt64, value);
                        break;

                    default:
                        Stop();
                        break;
                }

                _arguments.Add(result);

            }

            _sb.Append(_generator.WriteParameter(varName));

            return result;

        }

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Diagnostics.DebuggerStepThrough]
        private void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
                return;
            }

            throw new NotImplementedException();

        }

        private readonly StringBuilder _sb;
        private readonly List<DbParameter> _arguments;
        private readonly IQueryPredicateGenerator _generator;
        private readonly ObjectMapping _mapping;
        private readonly SqlManager _manager;
    }

}