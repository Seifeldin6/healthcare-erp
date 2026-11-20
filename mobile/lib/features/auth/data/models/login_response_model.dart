import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';
import 'package:json_annotation/json_annotation.dart';

part 'login_response_model.g.dart';

@JsonSerializable()
class LoginResponseModel extends LoginEntity {
  const LoginResponseModel({
    required super.token,
    required super.userId,
    required super.role,
    super.tenantId,
    super.patientId,
  });

  factory LoginResponseModel.fromJson(Map<String, dynamic> json) =>
      _$LoginResponseModelFromJson(json);
}
