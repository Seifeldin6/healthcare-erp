class LoginEntity {
  final String token;
  final String userId;
  final String role;
  final String? tenantId;
  final String? patientId;

  const LoginEntity({
    required this.token,
    required this.userId,
    required this.role,
    this.tenantId,
    this.patientId,
  });
}