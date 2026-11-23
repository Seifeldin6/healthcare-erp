class AppRegex {
  AppRegex._();

  static bool isEmailValid(String email) {
    return RegExp(
      r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$',
    ).hasMatch(email);
  }

  static bool isPasswordValid(String password) {
    return RegExp(r'^.{8,}$').hasMatch(password);
  }

  static bool isPhoneNumberValid(String phoneNumber) {
    final cleaned = phoneNumber.replaceAll(RegExp(r'\s+'), '');
    return RegExp(r'^(?:\+20|0)?1[0125][0-9]{8}$').hasMatch(cleaned);
  }

  static bool isNationalIdValid(String nationalId) {
    return RegExp(r'^[23][0-9]{13}$').hasMatch(nationalId);
  }

  static bool hasLowerCase(String password) {
    return RegExp(r'^(?=.*[a-z])').hasMatch(password);
  }

  static bool hasUpperCase(String password) {
    return RegExp(r'^(?=.*[A-Z])').hasMatch(password);
  }

  static bool hasNumber(String password) {
    return RegExp(r'^(?=.*?[0-9])').hasMatch(password);
  }

  static bool hasSpecialCharacter(String password) {
    return RegExp(r'^(?=.*?[#?!@$%^&*-])').hasMatch(password);
  }

  static bool hasMinLength(String password) {
    return RegExp(r'^(?=.{8,})').hasMatch(password);
  }
}
