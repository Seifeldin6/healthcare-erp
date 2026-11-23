import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/app_regex.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/core/widgets/app_text_form_field.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/login_error_banner.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/register_security_notice_banner.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/server_field_error_text.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterStepOneAccount extends StatefulWidget {
  final GlobalKey<FormState> formKey;
  final TextEditingController emailController;
  final TextEditingController phoneController;
  final TextEditingController passwordController;
  final TextEditingController confirmPasswordController;
  final VoidCallback onContinue;
  final String? serverEmailError;
  final String? serverPhoneError;
  final ValueChanged<String>? onEmailChanged;
  final ValueChanged<String>? onPhoneChanged;
  final bool isEditingFromReview;

  const RegisterStepOneAccount({
    super.key,
    required this.formKey,
    required this.emailController,
    required this.phoneController,
    required this.passwordController,
    required this.confirmPasswordController,
    required this.onContinue,
    this.serverEmailError,
    this.serverPhoneError,
    this.onEmailChanged,
    this.onPhoneChanged,
    this.isEditingFromReview = false,
  });

  @override
  State<RegisterStepOneAccount> createState() => _RegisterStepOneAccountState();
}

class _RegisterStepOneAccountState extends State<RegisterStepOneAccount> {
  bool _isPasswordObscure = true;
  bool _isConfirmPasswordObscure = true;

  Widget _buildVisibilityIcon({
    required bool isObscure,
    required VoidCallback onToggle,
  }) {
    return IconButton(
      onPressed: onToggle,
      icon: Icon(
        isObscure ? Icons.visibility_outlined : Icons.visibility_off_outlined,
        color: AppColors.muted,
        size: 20.sp,
      ),
    );
  }

  Widget _buildBottomLoginPrompt(BuildContext context, S s) {
    final promptText = widget.isEditingFromReview
        ? s.isThisYourAccount
        : s.alreadyHaveAccount;

    return Center(
      child: Text.rich(
        TextSpan(
          text: '$promptText ',
          style: AppTextStyles.body15MutedRegular,
          children: [
            TextSpan(
              text: s.logInAction,
              style: AppTextStyles.body15PrimaryBold,
              recognizer: TapGestureRecognizer()
                ..onTap = () =>
                    context.pushReplacementNamed(Routes.loginScreen),
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final hasEmailError = !widget.serverEmailError.isNullOrEmpty();
    final hasPhoneError = !widget.serverPhoneError.isNullOrEmpty();
    final hasAnyServerError = hasEmailError || hasPhoneError;

    return SingleChildScrollView(
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing24.w,
        vertical: AppDimensions.spacing16.h,
      ),
      child: Form(
        key: widget.formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (hasAnyServerError) ...[
              LoginErrorBanner(
                title: s.couldntCreateAccountTitle,
                message: s.couldntCreateAccountSubtitle,
              ),
              SizedBox(height: AppDimensions.spacing16.h),
            ],

            // 1. Email Field
            AppTextFormField(
              controller: widget.emailController,
              label: s.emailLabel,
              hintText: s.emailHint,
              textDirection: TextDirection.ltr,
              keyboardType: TextInputType.emailAddress,
              textInputAction: TextInputAction.next,
              hasError: hasEmailError,
              onChanged: widget.onEmailChanged,
              prefixIcon: Icon(
                Icons.mail_outline_rounded,
                color: hasEmailError ? AppColors.error : AppColors.muted,
                size: 20.sp,
              ),
              validator: (value) {
                if (value == null || value.trim().isEmpty) {
                  return s.emailRequiredError;
                }
                if (!AppRegex.isEmailValid(value.trim())) {
                  return s.invalidEmailError;
                }
                return null;
              },
            ),
            if (hasEmailError)
              ServerFieldErrorText(errorMessage: widget.serverEmailError!),
            SizedBox(height: AppDimensions.spacing16.h),

            // 2. Phone Field
            AppTextFormField(
              controller: widget.phoneController,
              label: s.phoneNumberLabel,
              hintText: s.phoneNumberHint,
              textDirection: TextDirection.ltr,
              keyboardType: TextInputType.phone,
              textInputAction: TextInputAction.next,
              hasError: hasPhoneError,
              onChanged: widget.onPhoneChanged,
              prefixIcon: Icon(
                Icons.phone_outlined,
                color: hasPhoneError ? AppColors.error : AppColors.muted,
                size: 20.sp,
              ),
              validator: (value) {
                if (value == null || value.trim().isEmpty) {
                  return s.phoneRequiredError;
                }
                if (!AppRegex.isPhoneNumberValid(value.trim())) {
                  return s.invalidPhoneError;
                }
                return null;
              },
            ),
            if (hasPhoneError)
              ServerFieldErrorText(errorMessage: widget.serverPhoneError!),
            SizedBox(height: AppDimensions.spacing16.h),

            // 3. Password Field
            AppTextFormField(
              controller: widget.passwordController,
              label: s.passwordLabel,
              hintText: s.createPasswordHint,
              textDirection: TextDirection.ltr,
              obscureText: _isPasswordObscure,
              textInputAction: TextInputAction.next,
              prefixIcon: Icon(
                Icons.lock_outline_rounded,
                color: AppColors.muted,
                size: 20.sp,
              ),
              suffixIcon: _buildVisibilityIcon(
                isObscure: _isPasswordObscure,
                onToggle: () =>
                    setState(() => _isPasswordObscure = !_isPasswordObscure),
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return s.passwordRequiredError;
                }
                if (!AppRegex.isPasswordValid(value)) {
                  return s.passwordLengthError;
                }
                return null;
              },
            ),
            SizedBox(height: AppDimensions.spacing16.h),

            // 4. Confirm Password Field
            AppTextFormField(
              controller: widget.confirmPasswordController,
              label: s.confirmPasswordLabel,
              hintText: s.reEnterPasswordHint,
              textDirection: TextDirection.ltr,
              obscureText: _isConfirmPasswordObscure,
              textInputAction: TextInputAction.done,
              prefixIcon: Icon(
                Icons.lock_outline_rounded,
                color: AppColors.muted,
                size: 20.sp,
              ),
              suffixIcon: _buildVisibilityIcon(
                isObscure: _isConfirmPasswordObscure,
                onToggle: () => setState(
                  () => _isConfirmPasswordObscure = !_isConfirmPasswordObscure,
                ),
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return s.confirmPasswordRequiredError;
                }
                if (value != widget.passwordController.text) {
                  return s.passwordsDoNotMatchError;
                }
                return null;
              },
            ),
            SizedBox(height: 20.h),

            // 5. Security Info Banner
            const RegisterSecurityNoticeBanner(),
            SizedBox(height: AppDimensions.spacing24.h),

            // 6. Continue Button
            SizedBox(
              width: double.infinity,
              child: AppPrimaryButton(
                text: widget.isEditingFromReview
                    ? s.continueToReviewButton
                    : s.continueButton,
                suffixIcon: Icons.arrow_forward_rounded,
                onPressed: () {
                  if (widget.formKey.currentState!.validate()) {
                    widget.onContinue();
                  }
                },
              ),
            ),
            SizedBox(height: AppDimensions.spacing16.h),

            // 7. Login Prompt
            _buildBottomLoginPrompt(context, s),
          ],
        ),
      ),
    );
  }
}
