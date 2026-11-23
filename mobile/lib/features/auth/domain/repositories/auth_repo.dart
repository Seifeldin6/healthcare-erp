import 'package:healthcare_app/core/networking/api_result.dart';
import 'package:healthcare_app/features/auth/data/models/register_patient_request_model.dart';
import 'package:healthcare_app/features/auth/domain/entities/login_entity.dart';

abstract class AuthRepo {
  Future<ApiResult<LoginEntity>> login({
    required String emailOrPhone,
    required String password,
  });

  Future<ApiResult<LoginEntity>> registerPatient(
    RegisterPatientRequestModel requestModel,
  );
}
