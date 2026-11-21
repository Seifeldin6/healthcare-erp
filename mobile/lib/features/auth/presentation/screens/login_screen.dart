import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/core/widgets/app_text_form_field.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/auth_state.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/login_cubit.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/auth_brand_header.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/auth_footer_prompt.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/login_error_banner.dart';
import 'package:healthcare_app/features/auth/presentation/widgets/patient_protal_notice.dart';
import 'package:healthcare_app/generated/l10n.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _emailOrPhoneController = TextEditingController();
  final _passwordController = TextEditingController();

  bool _isPasswordObscured = true;

  @override
  void dispose() {
    _emailOrPhoneController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  void _onSubmit() {
    if (!(_formKey.currentState?.validate() ?? false)) return;
    FocusScope.of(context).unfocus();
    context.read<LoginCubit>().login(
      emailOrPhone: _emailOrPhoneController.text,
      password: _passwordController.text,
    );
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Scaffold(
      backgroundColor: AppColors.surface,
      body: SafeArea(
        child: BlocConsumer<LoginCubit, AuthState>(
          listener: (context, state) {
            if (state is AuthSuccess) {
              context.pushReplacementNamed(Routes.homeScreen);
            }
          },
          builder: (context, state) {
            final isLoading = state is AuthLoading;
            final hasLoginError = state is AuthFailure;

            return CustomScrollView(
              slivers: [
                SliverFillRemaining(
                  hasScrollBody: false,
                  child: Padding(
                    padding: EdgeInsets.symmetric(
                      horizontal: AppDimensions.spacing24.w,
                      vertical: AppDimensions.spacing24.h,
                    ),
                    child: Form(
                      key: _formKey,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const AuthBrandHeader(),
                          SizedBox(height: AppDimensions.spacing32.h),
                          Text(
                            s.loginWelcomeTitle,
                            style: AppTextStyles.display30InkExtraBold,
                          ),
                          SizedBox(height: AppDimensions.spacing8.h),
                          Text(
                            s.loginWelcomeSubtitle,
                            style: AppTextStyles.body16BodyRegular,
                          ),
                          if (hasLoginError) ...[
                            SizedBox(height: AppDimensions.spacing24.h),
                            LoginErrorBanner(message: state.error),
                          ],
                          SizedBox(height: AppDimensions.spacing24.h),
                          AppTextFormField(
                            controller: _emailOrPhoneController,
                            label: s.emailOrPhoneLabel,
                            hintText: s.emailOrPhoneHint,
                            helperText: s.emailOrPhoneLtrHelper,
                            textDirection: TextDirection.ltr,
                            keyboardType: TextInputType.emailAddress,
                            textInputAction: TextInputAction.next,
                            hasError: hasLoginError,
                            prefixIcon: Icon(
                              Icons.person_outline_rounded,
                              color: AppColors.muted,
                              size: 20.sp,
                            ),
                            validator: (value) {
                              if (value == null || value.trim().isEmpty) {
                                return s.emailOrPhoneRequiredError;
                              }
                              return null;
                            },
                          ),
                          SizedBox(height: AppDimensions.spacing16.h),
                          AppTextFormField(
                            controller: _passwordController,
                            label: s.passwordLabel,
                            hintText: s.passwordHint,
                            obscureText: _isPasswordObscured,
                            textDirection: TextDirection.ltr,
                            textInputAction: TextInputAction.done,
                            hasError: hasLoginError,
                            prefixIcon: Icon(
                              Icons.lock_outline_rounded,
                              color: AppColors.muted,
                              size: 20.sp,
                            ),
                            suffixIcon: IconButton(
                              onPressed: () {
                                setState(() {
                                  _isPasswordObscured = !_isPasswordObscured;
                                });
                              },
                              icon: Icon(
                                _isPasswordObscured
                                    ? Icons.visibility_outlined
                                    : Icons.visibility_off_outlined,
                                color: AppColors.muted,
                                size: 20.sp,
                              ),
                            ),
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return s.passwordRequiredError;
                              }
                              return null;
                            },
                          ),
                          SizedBox(height: AppDimensions.spacing24.h),
                          AppPrimaryButton(
                            text: hasLoginError
                                ? s.tryAgainButton
                                : s.loginButton,
                            onPressed: isLoading ? null : _onSubmit,
                          ),
                          if (!hasLoginError) ...[
                            SizedBox(height: AppDimensions.spacing16.h),
                            const PatientPortalNotice(),
                          ],
                          const Spacer(),
                          SizedBox(height: AppDimensions.spacing24.h),
                          AuthFooterPrompt(
                            leadingText: s.newToShifa,
                            actionText: s.createAccount,
                            onActionTap: () {},
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            );
          },
        ),
      ),
    );
  }
}
