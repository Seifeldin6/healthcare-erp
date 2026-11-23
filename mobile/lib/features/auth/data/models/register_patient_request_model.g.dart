// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'register_patient_request_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RegisterPatientRequestModel _$RegisterPatientRequestModelFromJson(
  Map<String, dynamic> json,
) => RegisterPatientRequestModel(
  email: json['email'] as String,
  phoneNumber: json['phoneNumber'] as String,
  password: json['password'] as String,
  fullName: json['fullName'] as String,
  nationalId: json['nationalId'] as String,
  dateOfBirth: json['dateOfBirth'] as String,
  gender: json['gender'] as String,
  bloodType: json['bloodType'] as String?,
  weightKg: (json['weightKg'] as num?)?.toDouble(),
  heightCm: (json['heightCm'] as num?)?.toDouble(),
  emergencyContactName: json['emergencyContactName'] as String?,
  emergencyContactPhone: json['emergencyContactPhone'] as String?,
);

Map<String, dynamic> _$RegisterPatientRequestModelToJson(
  RegisterPatientRequestModel instance,
) => <String, dynamic>{
  'email': instance.email,
  'phoneNumber': instance.phoneNumber,
  'password': instance.password,
  'fullName': instance.fullName,
  'nationalId': instance.nationalId,
  'dateOfBirth': instance.dateOfBirth,
  'gender': instance.gender,
  'bloodType': ?instance.bloodType,
  'weightKg': ?instance.weightKg,
  'heightCm': ?instance.heightCm,
  'emergencyContactName': ?instance.emergencyContactName,
  'emergencyContactPhone': ?instance.emergencyContactPhone,
};
