import 'package:json_annotation/json_annotation.dart';

part 'register_patient_request_model.g.dart';

@JsonSerializable(includeIfNull: false)
class RegisterPatientRequestModel {
  final String email;
  final String phoneNumber;
  final String password;
  final String fullName;
  final String nationalId;
  final String dateOfBirth;
  final String gender;
  final String? bloodType;
  final double? weightKg;
  final double? heightCm;
  final String? emergencyContactName;
  final String? emergencyContactPhone;

  const RegisterPatientRequestModel({
    required this.email,
    required this.phoneNumber,
    required this.password,
    required this.fullName,
    required this.nationalId,
    required this.dateOfBirth,
    required this.gender,
    this.bloodType,
    this.weightKg,
    this.heightCm,
    this.emergencyContactName,
    this.emergencyContactPhone,
  });

  factory RegisterPatientRequestModel.fromJson(Map<String, dynamic> json) =>
      _$RegisterPatientRequestModelFromJson(json);

  Map<String, dynamic> toJson() => _$RegisterPatientRequestModelToJson(this);
}