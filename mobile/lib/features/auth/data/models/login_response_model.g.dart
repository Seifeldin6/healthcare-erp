// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login_response_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LoginResponseModel _$LoginResponseModelFromJson(Map<String, dynamic> json) =>
    LoginResponseModel(
      token: json['token'] as String,
      userId: json['userId'] as String,
      role: json['role'] as String,
      tenantId: json['tenantId'] as String?,
      patientId: json['patientId'] as String?,
    );

Map<String, dynamic> _$LoginResponseModelToJson(LoginResponseModel instance) =>
    <String, dynamic>{
      'token': instance.token,
      'userId': instance.userId,
      'role': instance.role,
      'tenantId': instance.tenantId,
      'patientId': instance.patientId,
    };
